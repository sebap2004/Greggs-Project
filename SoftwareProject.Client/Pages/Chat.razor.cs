using System.Net.Http.Json;
using System.Security.Claims;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Magic.IndexedDb;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using MudBlazor;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Models;
using SoftwareProject.Client.Providers;
using SoftwareProject.Client.Themes;

namespace SoftwareProject.Client.Pages;

public partial class Chat : ComponentBase
{
    private List<LocalTopicModel?> CachedLocalTopicList = new();
    private LocalTopicModel? ActiveLocalTopic { get; set; }
    
    private List<Topic> CachedOnlineTopicList = new();
    private Topic? ActiveOnlineTopic { get; set; }
    private bool inNewTopic { get; set; } = true;
    private List<LocalMessageModel> LocalMessages = new();
    private List<MessageDto> OnlineMessages = new();
    private int userID;
    
    private bool UseFake { get; set; }
    private bool isLocal { get; set; }
    private Variant localVariant => isLocal ? Variant.Filled : Variant.Outlined;
    private Variant onlineVariant => isLocal ? Variant.Outlined : Variant.Filled;
    private Language Language { get; set; } = Languages.English;
    private bool SummariseText { get; set; }
    private string SummariseTextLabel => SummariseText ? Icons.Material.Filled.Check : Icons.Material.Filled.Close;
    private Color SummariseButtonColor => SummariseText ? Color.Primary : Color.Default;

    private List<string> Fonts = new List<string>()
    {
        "Poppins",
        "Helvetica",
        "Arial",
        "sans-serif"
    };

    private string DropdownIcon =>
        QuickSettingsUp ? Icons.Material.Filled.ArrowDropUp : Icons.Material.Filled.ArrowDropDown;

    private bool QuickSettingsUp { get; set; }
    private bool SendingDisabled { get; set; }
    private string Question { get; set; }

    private bool _drawerOpen = true;
    private bool _isDarkMode = true;
    private MudTheme? _theme = null;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _theme = new DefaultTheme();
        isLocal = true;
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (!user.Identity!.IsAuthenticated)
        {
            // NavigationManager.NavigateTo($"/access-denied/{Uri.EscapeDataString("notauthorized")}");
            // return;
        }
        await LoadLocalTopicsFromDB();
        string userIDString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";
        userID = Int32.Parse(userIDString);
        CachedOnlineTopicList = await TopicClient.GetTopics(userID);
    }


    private async Task PromptEnterHandler(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            if (SendingDisabled) return;
            if (e.ShiftKey) return;
            await Submit();
        }
    }

    private async Task Submit()
    {
        Console.WriteLine("Starting submission");
        if (Question == "") return;
        string tempQuestion = Question;
        string translatedQuestion = "";
        SendingDisabled = true;
        
        var offlineMessage = new LocalMessageModel
        {
            content = tempQuestion,
            isUser = true
        };

        var onlineMessage = new MessageDto
        {
            AiResponse = false,
            MessageText = tempQuestion,
        };
        
        if (isLocal)
        {
            
            Console.WriteLine("Created new message");
            LocalMessages.Add(offlineMessage);
            if (ActiveLocalTopic == null)
            {
                ActiveLocalTopic = new LocalTopicModel
                {
                    Topic = offlineMessage.content,
                    messages = new List<LocalMessageModel>()
                };
                ActiveLocalTopic.messages.Add(offlineMessage);
            }
            else
            {
                ActiveLocalTopic.messages.Add(offlineMessage);
                await AddLocalMessage();
            }

            StateHasChanged();
        }
        else
        {
            if (ActiveOnlineTopic == null)
            {
                var newTopic = new Topic()
                {
                    account_id = userID,
                    topicname = onlineMessage.MessageText
                };
                var createdTopic = await TopicClient.CreateTopic(newTopic);
                ActiveOnlineTopic = createdTopic;
                CachedOnlineTopicList.Add(ActiveOnlineTopic);
                onlineMessage.TopicId = ActiveOnlineTopic.topic_id;
                OnlineMessages.Add(onlineMessage);
                await MessageClient.CreateMessage(onlineMessage);
                StateHasChanged();
            }
            else
            {
                onlineMessage.TopicId = ActiveOnlineTopic.topic_id;
                OnlineMessages.Add(onlineMessage);
                var creationAttempt = await MessageClient.CreateMessage(onlineMessage);
                Console.WriteLine("Message created status: " + creationAttempt);
            }
        }

        if (Language != Languages.English)
        {
            Console.WriteLine("Translating question: " + (SummariseText ? "SUMMARISE THIS TEXT: " : "") + tempQuestion +
                              " to " + Language.Name);
            var translation = await http.PostAsJsonAsync("api/translate", new TranslateRequest
                {
                    Text = tempQuestion,
                    Language = Language.Code
                }
            );
            translatedQuestion = await translation.Content.ReadAsStringAsync();
            tempQuestion = translatedQuestion;
        }

        Question = "";

        // Get AI response
        Console.WriteLine("Sending message to AI: " + tempQuestion + "");
        string response = await ai.GetMessage((SummariseText ? "SUMMARISE THIS TEXT: " : "") + tempQuestion, UseFake);
        var aiLocalMessage = new LocalMessageModel
        {
            content = response,
            isUser = false
        };
        var aiOnlineMessage = new MessageDto
        {
            AiResponse = true,
            MessageText = response,
            TopicId = ActiveOnlineTopic?.topic_id ?? 0
        };
        Console.WriteLine("AI response: " + response + "");
        LocalMessages.Add(aiLocalMessage);

        if (isLocal)
        {
            Console.WriteLine("Adding message to local topic: " + aiLocalMessage.content + "");
            ActiveLocalTopic?.messages.Add(aiLocalMessage);
            await AddLocalMessage();
        }
        else
        {
            Console.WriteLine("Adding message to online topic: " + aiOnlineMessage.MessageText + "");
            OnlineMessages.Add(aiOnlineMessage);
            var createAttempt = await MessageClient.CreateMessage(aiOnlineMessage);
            Console.WriteLine("Message send attempt: " + createAttempt);
        }

        Question = "";
        SendingDisabled = false;
        Console.WriteLine("Complete!");
        StateHasChanged();
    }
    

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void DarkModeToggle()
    {
        _isDarkMode = !_isDarkMode;
    }

    private async Task CheckAuthenticationState()
    {
        Console.WriteLine("Starting check");
        var check = await AuthStateProvider.GetAuthenticationStateAsync();
        string? id = check.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine(check.User);
    }

    private void NewTopic()
    {
        if (isLocal)
        {
            Question = "";
            ActiveLocalTopic = null;
            LocalMessages.Clear();
        }
        else
        {
            Question = "";
            ActiveOnlineTopic = null;
            OnlineMessages.Clear();
        }
    }

    public string DarkLightModeButtonIcon => _isDarkMode switch
    {
        true => Icons.Material.Rounded.LightMode,
        false => Icons.Material.Outlined.DarkMode,
    };

    private async Task Logout()
    {
        var logoutattempt = await http.PostAsync("api/authentication/logout", null);
        if (logoutattempt.IsSuccessStatusCode)
        {
            if (AuthStateProvider is CookieAuthStateProvider customProvider)
            {
                Snackbar.Add("You have been logged out.", Severity.Success);
                NavigationManager.NavigateTo("/");
                customProvider.NotifyAuthenticationStateChanged();
            }
        }
    }

    private void UpdateActiveLocalTopic(LocalTopicModel topic)
    {
        Console.WriteLine("Updating active topic: " + topic.Topic + "");
        ActiveLocalTopic = topic;
        LocalMessages.Clear();
        LocalMessages.AddRange(topic.messages);
    }

    private async Task UpdateActiveOnlineTopic(Topic topic)
    {
        Console.WriteLine("Updating active online topic: " + topic.topic_id);
        ActiveOnlineTopic = topic;
        OnlineMessages.Clear();
        var messagesFromDB = await MessageClient.GetMessages(topic.topic_id);
        OnlineMessages.AddRange(messagesFromDB);
    }

    private async Task LoadLocalTopicsFromDB()
    {
        try
        {
            Console.WriteLine("Started loading topics from DB");
            var allRecords = await magicDb.Query<Topics>();
            var allTopics = await allRecords.ToListAsync();
            CachedLocalTopicList = allTopics
                .Where(t => t.Topic != null && !string.IsNullOrEmpty(t.Topic.Topic))
                .Select(t => t.Topic)
                .ToList();
            Console.WriteLine("Loaded " + CachedLocalTopicList.Count + " topics from DB");
            if (CachedLocalTopicList.Count > 0 && ActiveLocalTopic == null)
            {
                LocalMessages = new List<LocalMessageModel>(ActiveLocalTopic.messages);
            }
            StateHasChanged();
            Console.WriteLine("Finished loading topics from DB");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading topics: {ex}");
        }
    }

    private async Task AddLocalMessage()
    {
        Console.WriteLine("Started adding local message to DB: " + ActiveLocalTopic?.GUID);
        IMagicQuery<Topics> topicQuery = await magicDb.Query<Topics>();
        // Check if this is a new topic (no ID yet) or an existing one
        if (string.IsNullOrWhiteSpace(ActiveLocalTopic.GUID))
        {
            Console.WriteLine("No ID. Creating a new Topic and Adding to databse");
            var newTopicEntry = new Topics
            {
                Topic = ActiveLocalTopic,
                GUID = RandomIDGenerator.GenerateRandomID()
            };
            newTopicEntry.Topic.GUID = newTopicEntry.GUID;
            await topicQuery.AddAsync(newTopicEntry);
            var addedTopic = await topicQuery.FirstOrDefaultAsync(t => t.GUID == ActiveLocalTopic.GUID);
            if (addedTopic != null)
            {
                ActiveLocalTopic.GUID = addedTopic.GUID;
                Console.WriteLine("Updated local GUID to " + ActiveLocalTopic.GUID);
            }
        }
        else
        {
            Console.WriteLine("Searching for topic with GUID " + ActiveLocalTopic.GUID + "");
            var allRecords = await magicDb.Query<Topics>();
            var allTopics = await allRecords.ToListAsync();
            var existingTopic = allTopics.FirstOrDefault(x => x.GUID == ActiveLocalTopic.GUID);
            Console.WriteLine(existingTopic);
            if (existingTopic != null)
            {
                Console.WriteLine("Found existing topic with GUID " + ActiveLocalTopic.GUID);
                existingTopic.Topic = ActiveLocalTopic;
                await topicQuery.UpdateAsync(existingTopic);
            }
        }

        await LoadLocalTopicsFromDB();
        Console.WriteLine("Finished adding local message");
    }
}
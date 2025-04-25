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
    private List<IndexedTopicModel?> LocalTopics = new();
    private IndexedTopicModel? ActiveLocalTopic { get; set; }

    private bool inNewTopic { get; set; } = true;

    private List<MessageModel> messages = new();
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
    
    // Database Variables
    private Topic topic = new Topic();
    private Message message = new Message();
    // TODO: REMOVE THIS WHEN WORKING CORRECTLY
    bool TOPICTEST = false;

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
        }

        await LoadLocalTopicsFromDB();
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

        var message = new MessageModel
        {
            content = tempQuestion,
            isUser = true
        };
        Console.WriteLine("Created new message");
        messages.Add(message);

        if (isLocal)
        {
            if (ActiveLocalTopic == null)
            {
                ActiveLocalTopic = new IndexedTopicModel
                {
                    Topic = message.content,
                    messages = new List<MessageModel>()
                };
                ActiveLocalTopic.messages.Add(message);
            }
            else
            {
                ActiveLocalTopic.messages.Add(message);
                await AddLocalMessage();
            }

            StateHasChanged();
        }
        else
        {
            // Do database
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
        var aiMessage = new MessageModel
        {
            content = response,
            isUser = false
        };
        Console.WriteLine("AI response: " + response + "");
        messages.Add(aiMessage);

        if (isLocal)
        {
            Console.WriteLine("Adding message to local topic: " + aiMessage.content + "");
            ActiveLocalTopic?.messages.Add(aiMessage);
            await AddLocalMessage();
        }
        else
        {
            // Do database
        }

        Question = "";
        SendingDisabled = false;
        Console.WriteLine("Complete!");
        StateHasChanged();

        if (TOPICTEST == false)
        {
            await UploadTopic(tempQuestion);
        }

        await UploadMessage(0, tempQuestion);
        await UploadMessage(1, response);
    }
    
    /// <summary>
    /// Upload the topic to the database.
    /// If the input provided is over 200 characters then it will limit the length of the string.
    /// Also assigns the user ID to the topic so that it can only be called for specific users.
    /// </summary>
    /// <param name="tempQuestion">Gets the user input</param>
    /// <param name="editContext">Tracks the changes made in the form</param>
    private async Task UploadTopic(string tempQuestion)
    {
        string topicName;
        int topicNameMaxLength = 200;
        
        if (tempQuestion.Length > topicNameMaxLength)
        {
            topicName = tempQuestion.Substring(0, topicNameMaxLength);
        }
        else
        {
            topicName = tempQuestion;
        }
        topic.topicname = topicName;
        
        var checkId = await AuthStateProvider.GetAuthenticationStateAsync();
        string? userId = checkId.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        topic.account_id = Int32.Parse(userId);
        
        try
        {
            await topicService.CreateTopic(topic);
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Error Connecting to Database: \n{e.Message}");
        }
    }

    /// <summary>
    /// Upload the messages to the database. Will take both AI and Human responses.
    /// </summary>
    /// <param name="responseType">AI or Human identifier. 0 for Human. 1 for AI.</param>
    /// <param name="text">Stores the message to be uploaded.</param>
    private async Task UploadMessage(int responseType, string text)
    {
        DateTime currentTime = DateTime.Now;
        
        message.airesponse = responseType;
        message.messagetext = text;
        message.timesent = currentTime;
        
        try
        {
            await messageService.CreateMessage(message);
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Error Connecting to Database: \n{e.Message}");
        }
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
            messages.Clear();
        }
        else
        {
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
                customProvider.NotifyAuthenticationStateChanged();
                Snackbar.Add("You have been logged out.", Severity.Success);
                NavigationManager.NavigateTo("/");
            }
        }
    }

    private void UpdateActiveLocalTopic(IndexedTopicModel topic)
    {
        Console.WriteLine("Updating active topic: " + topic.Topic + "");
        ActiveLocalTopic = topic;
        messages.Clear();
        messages.AddRange(topic.messages);
    }

    private async Task LoadLocalTopicsFromDB()
    {
        try
        {
            Console.WriteLine("Started loading topics from DB");
            var allRecords = await magicDb.Query<Topics>();
            var allTopics = await allRecords.ToListAsync();
            LocalTopics = allTopics
                .Where(t => t.Topic != null && !string.IsNullOrEmpty(t.Topic.Topic))
                .Select(t => t.Topic)
                .ToList();
            Console.WriteLine("Loaded " + LocalTopics.Count + " topics from DB");
            if (LocalTopics.Count > 0 && ActiveLocalTopic == null)
            {
                messages = new List<MessageModel>(ActiveLocalTopic.messages);
            }

            StateHasChanged();
            Console.WriteLine("Finished loading topics from DB");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading topics: {ex.Message}");
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

    private async Task AddLocalMessageTest()
    {
        MessageModel message = new MessageModel
        {
            content = "Hello! " + DateTime.Now,
            isUser = true
        };
        IMagicQuery<Topics> topicQuery = await magicDb.Query<Topics>();

        await topicQuery.AddAsync(new Topics
        {
            Topic = ActiveLocalTopic ?? new IndexedTopicModel
            {
                Topic = "Test " + DateTime.Now,
                messages = new List<MessageModel>
                {
                    message
                }
            }
        });
    }
}
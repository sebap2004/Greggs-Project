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
    /// <summary>
    /// Local Topic List cached in browser
    /// </summary>
    private List<LocalTopicModel?> CachedLocalTopicList = new();
    
    /// <summary>
    /// Current Local Topic
    /// </summary>
    private LocalTopicModel? ActiveLocalTopic { get; set; }
    
    /// <summary>
    /// Online topic list cached in browser
    /// </summary>
    private List<Topic> CachedOnlineTopicList = new();
    
    /// <summary>
    /// Current Active Online Topic
    /// </summary>
    private Topic? ActiveOnlineTopic { get; set; }
    
    
    /// <summary>
    /// List storing local messages of the current active local topic.
    /// </summary>
    private List<LocalMessageModel> LocalMessages = new();
    
    /// <summary>
    /// List storing cached messages of an online topic on the database.
    /// </summary>
    private List<MessageDto> OnlineMessages = new();
    
    /// <summary>
    /// Cached user ID stored for creating new topics.
    /// </summary>
    private int userID;
    
    /// <summary>
    /// If true, Ai service will skip using a real gemini API and return a fake response.
    /// </summary>
    private bool UseAI { get; set; }
    
    /// <summary>
    /// If the user is in local mode or not.
    /// </summary>
    private bool isLocal { get; set; }
    
    /// <summary>
    /// Button styling variant for if the user is in local mode.
    /// </summary>
    private Variant localVariant => isLocal ? Variant.Filled : Variant.Outlined;
    
    /// <summary>
    /// Button styling variant for if the user is in online mode.
    /// </summary>
    private Variant onlineVariant => isLocal ? Variant.Outlined : Variant.Filled;
    
    /// <summary>
    /// Language to translate the question to.
    /// </summary>
    private Language Language { get; set; } = Languages.English;
    
    /// <summary>
    /// If the prompt has a prefix applied asking to summarise a big text block
    /// <code>
    /// // Bool will add this
    /// "SUMMARISE THIS TEXT: "
    /// </code>
    /// </summary>
    private bool SummariseText { get; set; }
    
    /// <summary>
    /// Icon for if the summarise feature is enabled.
    /// </summary>
    private string SummariseTextLabel => SummariseText ? Icons.Material.Filled.Check : Icons.Material.Filled.Close;
    
    /// <summary>
    /// Color for is the summarise feature is enabled.
    /// </summary>
    private Color SummariseButtonColor => SummariseText ? Color.Primary : Color.Default;

    
    /// <summary>
    /// List of fonts available in the font changer.
    /// </summary>
    private readonly List<string> Fonts =
    [
        "Poppins",
        "Helvetica",
        "Arial",
        "sans-serif"
    ];

    /// <summary>
    /// Icon for if the dropdown is visible or not.
    /// </summary>
    private string DropdownIcon =>
        QuickSettingsUp ? Icons.Material.Filled.ArrowDropUp : Icons.Material.Filled.ArrowDropDown;

    /// <summary>
    /// Boolean controlling quick settings dropdown.
    /// </summary>
    private bool QuickSettingsUp { get; set; }
    
    /// <summary>
    /// Boolean that is set to true if a request is being submitted.
    /// </summary>
    private bool SendingDisabled { get; set; }
    
    /// <summary>
    /// Binding property to the question text box.
    /// </summary>
    private string Question { get; set; }

    /// <summary>
    /// Binding property to the sidebar open state.
    /// </summary>
    private bool _drawerOpen = true;
    
    /// <summary>
    /// Binding property to the dark mode state.
    /// </summary>
    private bool _isDarkMode = true;
    
    /// <summary>
    /// Binding property to the theme state.
    /// </summary>
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
        Console.WriteLine("User ID: " + userID);
        await LoadOnlineTopicsFromDB();
    }

    


    /// <summary>
    /// Event listener.
    /// Checks if the enter key is pressed from a keyboard event.
    /// <remarks>Shift key check added to allow multilines without breaking.</remarks>
    /// </summary>
    /// <param name="e">Keyboard event receieved</param>
    private async Task PromptEnterHandler(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            if (SendingDisabled) return;
            if (e.ShiftKey) return;
            await Submit();
        }
    }

    /// <summary>
    /// This method submits a message to the AI
    /// Stores the responses locally or on an online database depending on which mode the user has chosen.
    /// </summary>
    private async Task Submit()
    {
        Console.WriteLine("Starting submission");
        
        // Immediately ends if the question box is empty
        if (Question == "") return;
        
        // Initialises temporary question string so the Question box can be safely emptied.
        string tempQuestion = Question;
        Question = "";
        
        // Disables sending controls while processing.
        SendingDisabled = true;
        
        // Creates offline message model for if user is in offline mode.
        var offlineMessage = new LocalMessageModel
        {
            content = tempQuestion,
            isUser = true
        };

        // Creates online message model for if user is in offline mode.
        var onlineMessage = new MessageDto
        {
            AiResponse = false,
            MessageText = tempQuestion,
        };
        
        // If user is in offline mode
        if (isLocal)
        {
            // Add offline messages to cached offline message list
            LocalMessages.Add(offlineMessage);
            
            // If active local topic is null, either due to lack of topics or starting a new one,
            // create a new topic and add the offline message to it.
            if (ActiveLocalTopic == null)
            {
                ActiveLocalTopic = new LocalTopicModel
                {
                    Topic = offlineMessage.content,
                    userID = userID,
                    messages =
                    [
                        offlineMessage
                    ]
                };
            }
            else
            {
                ActiveLocalTopic.messages.Add(offlineMessage);
                await AddLocalMessage(); // Add local message to the local indexed database.
            }

            // Call for rerender.
            StateHasChanged();
        }
        // If user is in online mode
        else
        {
            // If active online topic is null, either due to lack of topics or starting a new one,
            // create a new topic and add the online message to it.
            if (ActiveOnlineTopic == null)
            {
                // Create new topic object
                var newTopic = new Topic()
                {
                    account_id = userID,
                    topicname = onlineMessage.MessageText
                };
                // Create topic in database
                var createdTopic = await TopicClient.CreateTopic(newTopic);
                ActiveOnlineTopic = createdTopic; // Set the active topic to the one created in the database
                CachedOnlineTopicList.Add(ActiveOnlineTopic); // Add to topic list
                onlineMessage.TopicId = ActiveOnlineTopic.topic_id; // Set pending online message to the topic ID
                OnlineMessages.Add(onlineMessage); // Add to local message list
                await MessageClient.CreateMessage(onlineMessage); // Add message to database
                StateHasChanged();
            }
            else
            {
                // If active topic is not null, just add the message to the list and upload to database.
                onlineMessage.TopicId = ActiveOnlineTopic.topic_id;
                OnlineMessages.Add(onlineMessage);
                var creationAttempt = await MessageClient.CreateMessage(onlineMessage);
                Console.WriteLine("Message created status: " + creationAttempt);
            }
        }

        // If the language is NOT english, call translation API to translate prompt.
        // Translation of prompt facilitates response to prompt in the same language.
        if (Language != Languages.English)
        {
            Console.WriteLine("Translating question: " + (SummariseText ? "SUMMARISE THIS TEXT: " : "") + tempQuestion +
                              " to " + Language.Name);
            var translation = await http.PostAsJsonAsync("api/translate", new TranslateRequest
                {
                    Text = tempQuestion,
                    Language = Language.Code
                }
            ); // Call google API
            var translatedQuestion = await translation.Content.ReadAsStringAsync(); // Get response
            tempQuestion = translatedQuestion; // Override tempQuestion with the translated question. This one is not stored in the database.
        }
        
        // Get AI response
        Console.WriteLine("Sending message to AI: " + tempQuestion + "");
        string response = await ai.GetMessage((SummariseText ? "SUMMARISE THIS TEXT: " : "") + tempQuestion, !UseAI);
        
        // Store AI response in local and online message models.
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

        // Add responses to respective databases
        if (isLocal)
        {
            Console.WriteLine("Adding message to local topic: " + aiLocalMessage.content + "");
            ActiveLocalTopic?.messages.Add(aiLocalMessage);
            LocalMessages.Add(aiLocalMessage);
            await AddLocalMessage();
        }
        else
        {
            Console.WriteLine("Adding message to online topic: " + aiOnlineMessage.MessageText + "");
            OnlineMessages.Add(aiOnlineMessage);
            var createAttempt = await MessageClient.CreateMessage(aiOnlineMessage);
            Console.WriteLine("Message send attempt: " + createAttempt);
        }
        SendingDisabled = false; // Re-enable sending  controls
        Console.WriteLine("Complete!");
        StateHasChanged();
    }
    
    /// <summary>
    /// Method for opening side drawer.
    /// </summary>
    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    /// <summary>
    /// Method for toggling dark mode.
    /// </summary>
    private void DarkModeToggle()
    {
        _isDarkMode = !_isDarkMode;
    }
    
    /// <summary>
    /// Flushes out active topic caches to facilitate creating a new topic.
    /// </summary>
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

    /// <summary>
    /// Expression bodied field to display light and dark mode icons
    /// </summary>
    public string DarkLightModeButtonIcon => _isDarkMode switch
    {
        true => Icons.Material.Rounded.LightMode,
        false => Icons.Material.Outlined.DarkMode,
    };

    
    /// <summary>
    /// Call the logout api to sign the user out of their account.
    /// </summary>
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
                StateHasChanged();
            }
        }
    }

    
    /// <summary>
    /// Update the active topic in local mode.
    /// Triggers rerendering of chatbox window to populate with new topic items
    /// </summary>
    /// <param name="topic">Topic to change to</param>
    private void UpdateActiveLocalTopic(LocalTopicModel topic)
    {
        Console.WriteLine("Updating active topic: " + topic.Topic + "");
        ActiveLocalTopic = topic;
        LocalMessages.Clear();
        LocalMessages.AddRange(topic.messages);
    }

    /// <summary>
    /// Update the active topic in online mode.
    /// Triggers rerendering of chatbox window to populate with new topic items
    /// </summary>
    /// <param name="topic">Topic to change to</param>
    private async Task UpdateActiveOnlineTopic(Topic topic)
    {
        Console.WriteLine("Updating active online topic: " + topic.topic_id);
        ActiveOnlineTopic = topic;
        OnlineMessages.Clear();
        var messagesFromDB = await MessageClient.GetMessages(topic.topic_id);
        OnlineMessages.AddRange(messagesFromDB);
    }


    /// <summary>
    /// Loads all local topics from the browser's database into the cached local topic list.
    /// </summary>
    private async Task LoadLocalTopicsFromDB()
    {
        try
        {
            Console.WriteLine("Started loading topics from DB");
            var allRecords = await magicDb.Query<Topics>();
            var allTopics = await allRecords.ToListAsync();
            CachedLocalTopicList = allTopics
                .Where(t => t.Topic != null && !string.IsNullOrEmpty(t.Topic.Topic) && t.userID == userID)
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
    
    private async Task LoadOnlineTopicsFromDB()
    {
        CachedOnlineTopicList = await TopicClient.GetTopics(userID);
    }

    /// <summary>
    /// Add a local message into the local database system.
    /// <remarks>If there is no active topic, this method will create a new one and assign the message to that topic.</remarks>
    /// </summary>
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
                userID = userID,
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

    private async Task DeleteLocalTopic(LocalTopicModel topic)
    {
        var allRecords = await magicDb.Query<Topics>();
        var allTopics = await allRecords.ToListAsync();
        var topicToDelete = allTopics.FirstOrDefault(t => t.GUID == topic.GUID);
        if (topicToDelete != null)
        {
            await allRecords.DeleteAsync(topicToDelete);
            CachedLocalTopicList.Remove(topic);
            await LoadLocalTopicsFromDB();
        }
    }

    private async Task DeleteOnlineTopic(Topic topic)
    {
        await TopicClient.DeleteTopic(topic.topic_id);
        CachedOnlineTopicList.Remove(topic);
        await LoadOnlineTopicsFromDB();
    }
}
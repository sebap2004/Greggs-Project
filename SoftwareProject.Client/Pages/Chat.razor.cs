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
using Newtonsoft.Json;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Models;
using SoftwareProject.Client.Providers;
using SoftwareProject.Client.SiteSettings;
using SoftwareProject.Client.Themes;

namespace SoftwareProject.Client.Pages;

public partial class Chat : ComponentBase
{
    /// <summary>
    /// Local Topic List cached in browser
    /// </summary>
    private List<LocalTopicModel?> CachedLocalTopicList = new();

    
    /// <summary>
    /// Current active text styling
    /// </summary>
    private TextStyling activeTextStyling;
    
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
    /// Current active text size. Setting this value triggers an update of the site's text size.
    /// </summary>
    private TextSizeEnum CurrentTextSize
    {
        get => currentTextSize;
        set
        {
            currentTextSize = value;
            UpdateTextSize(value);
            UpdateOnlineSettings(CachedSettings);
        }
    }
    
    private TextSizeEnum currentTextSize = TextSizeEnum.Normal;

    /// <summary>
    /// Specifies the number of recent messages to consider for generating the AI conversation context.
    /// </summary>
    private int ContextLength { get; set; } = 5;

    /// <summary>
    /// Controls how the AI will respond to a message.
    /// </summary>
    private ResponseTypes CurrentResponseType;

    /// <summary>
    /// Hides site content if content is loading.
    /// </summary>
    private bool isLoading;
    
    /// <summary>
    /// Current font to be used in the website. Setting this value will trigger an update to the site's fonts.
    /// </summary>
    private TextFontGroup CurrentTextFontGroup
    {
        get => currentTextFontGroup;
        set
        {
            currentTextFontGroup = value;
            UpdateTextFont(currentTextFontGroup);
            UpdateOnlineSettings(CachedSettings);
        }
    }

    private TextFontGroup currentTextFontGroup = TextFonts.Default;
    
    /// <summary>
    /// Button styling variant for if the user is in local mode.
    /// </summary>
    private Variant localVariant => isLocal ? Variant.Filled : Variant.Outlined;
    
    /// <summary>
    /// Button styling variant for if the user is in online mode.
    /// </summary>
    private Variant onlineVariant => isLocal ? Variant.Outlined : Variant.Filled;
    
    /// <summary>
    /// Button styling for if the user is in Detailed Response mode.
    /// </summary>
    private Variant detailedResponseTypeVariant => CurrentResponseType == ResponseTypes.Long ? Variant.Filled : Variant.Outlined;
    
    /// <summary>
    /// Button styling for if the user is in Concise Response mode.
    /// </summary>
    private Variant conciseResponseTypeVariant => CurrentResponseType == ResponseTypes.Short ? Variant.Filled : Variant.Outlined;
    
    /// <summary>
    /// Button styling for if the user is in formal response mode.
    /// </summary>
    private Variant formalResponseTypeVariant => CurrentResponseType == ResponseTypes.Formal ? Variant.Filled : Variant.Outlined;
    
    /// <summary>
    /// Language to translate the question to.
    /// </summary>
    private Language Language
    {
        get => language;
        set
        {
            language = value;
            CachedSettings.language = Language.Code;
            UpdateOnlineSettings(CachedSettings);
        }
    } 
    
    private Language language = Languages.English;
    
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

    private Settings CachedSettings { get; set; } = new Settings();

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
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            _isDarkMode = value;
            CachedSettings.darkmode = _isDarkMode;
            UpdateOnlineSettings(CachedSettings);
        }
    }
    
    
    private bool _isDarkMode = true;
    
    /// <summary>
    /// Binding property to the theme state.
    /// </summary>
    private DefaultTheme? _theme = null;
    
    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
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
        string userIDString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";
        userID = Int32.Parse(userIDString);
        await InitializeUserSettings();
        await LoadLocalTopicsFromDB();
        await LoadOnlineTopicsFromDB();
        isLoading = false;
    }


    /// <summary>
    /// Loads user settings and updates the application state to reflect the user's saved preferences.'
    /// If no settings are found for the user, creates default settings for the user
    /// in the settings service.
    /// </summary>
    private async Task InitializeUserSettings()
    {
        CachedSettings = await SettingsClient.GetSettings(userID);
        if (CachedSettings.account_id == 0)
        {
            await SettingsClient.CreateSettings(userID);
            CachedSettings = await SettingsClient.GetSettings(userID);
        }

        IsDarkMode = CachedSettings.darkmode;

        switch (CachedSettings.language)
        {
            case ("en-US"):
                Language = Languages.English;
                break; 
            case ("fr-FR"):
                Language = Languages.French;
                break;
            case ("de-DE"):
                Language = Languages.German;
                break;
            case ("es-ES"):
                Language = Languages.Spanish;
                break;
            case ("ja-JP"):
                Language = Languages.Japanese;
                break;
            case ("zh-CN"):
                Language = Languages.Chinese;
                break;
        }

        switch (CachedSettings.fontsize)
        {
            case 0:
                CurrentTextSize = TextSizeEnum.VerySmall;
                break;
            case 1:
                CurrentTextSize = TextSizeEnum.Small;
                break;
            case 2:
                CurrentTextSize = TextSizeEnum.Normal;
                break;
            case 3:
                CurrentTextSize = TextSizeEnum.Large;
                break;
            case 4:
                CurrentTextSize = TextSizeEnum.VeryLarge;
                break;
        }

        switch (CachedSettings.font)
        {
            case 0:
                CurrentTextFontGroup = TextFonts.Default;
                break;
            case 1:
                CurrentTextFontGroup = TextFonts.Dyslexia;
                break;
            case 2:
                CurrentTextFontGroup = TextFonts.Arial;
                break;
        }
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
            isHuman = true
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

        string response = "";

        
        // If summarise text enabled, Just use the last message.
        if (SummariseText)
        {
            response = await ai.GetMessage((SummariseText ? "SUMMARISE THIS TEXT: " : "") + tempQuestion, !UseAI);
        }
        // If summarise text enabled, use the latest N amount of messages, where N is the set ContextLength
        else
        {
            string responseType = "";
            // Depending on the response types, the AI is prompted to answer differently.
            switch (CurrentResponseType)
            {
                case ResponseTypes.Long:
                    responseType = "DETAILED AND LONG";
                    break;
                case ResponseTypes.Short:
                    responseType = "SHORT AND CONCISE";
                    break;
                case ResponseTypes.Formal:
                    responseType = "FORMAL AND POLITE";
                    break;
            }
            if (isLocal)
            {
                string context = "You are a helpful AI assistant. YOU ANSWER QUESTIONS IN A " + responseType + " MANNER. Here is the conversation history so far: ";
                foreach (var message in LocalMessages.TakeLast(ContextLength))
                {
                    if (message.isHuman)
                    {
                        context += "Human: " + message.content + " \n";
                    }
                    else
                    {
                        context += "AI: " + message.content + " \n";
                    }
                }
                context += "DO NOT REFERENCE THIS CONTEXT IN YOUR RESPONSE. Based on this previous context, answer this question: " + offlineMessage.content + " \n";
                Console.WriteLine(context);
                response = await ai.GetMessage(context, !UseAI);
            }
            else
            {
                string context = "You are a helpful AI assistant. YOU ANSWER QUESTIONS IN A " + responseType + " MANNER. Here is the conversation history so far: ";
                foreach (var message in OnlineMessages.TakeLast(ContextLength))
                {
                    if (message.AiResponse)
                    {
                        context += "AI: " + message.MessageText + " \n";
                    }
                    else
                    {
                        context += "Human: " + message.MessageText + " \n";
                    }
                }
                context += "DO NOT REFERENCE THIS CONTEXT IN YOUR RESPONSE. Based on this previous context, answer this question: " + offlineMessage.content + " \n";
                Console.WriteLine(context);
                response = await ai.GetMessage(context, !UseAI);
            }
        }
        
        // Store AI response in local and online message models.
        var aiLocalMessage = new LocalMessageModel
        {
            content = response,
            isHuman = false
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
        IsDarkMode = !IsDarkMode;
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
                .Where(t => t != null && !string.IsNullOrEmpty(t.Topic.Topic) && t.userID == userID)
                .Select(t => t.Topic)
                .ToList();
            Console.WriteLine("Loaded " + CachedLocalTopicList.Count + " topics from DB");
            if (CachedLocalTopicList.Count > 0 && ActiveLocalTopic == null)
            {
                if (ActiveLocalTopic?.messages != null)
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

    
    /// <summary>
    /// Deletes an online topic from the database.
    /// </summary>
    /// <param name="topic">Topic to be deleted.</param>
    private async Task DeleteOnlineTopic(Topic topic)
    {
        await TopicClient.DeleteTopic(topic.topic_id);
        CachedOnlineTopicList.Remove(topic);
        await LoadOnlineTopicsFromDB();
    }

    
    /// <summary>
    /// Update the active font used in the website.
    /// </summary>
    /// <param name="group">Font group to change to.</param>
    private void UpdateTextFont(TextFontGroup group)
    {
        Console.WriteLine("Updated text group to " + group.Id + "");
        _theme.TextStyling.FontGroup = group;
        _theme.UpdateTheme();
        activeTextStyling = new TextStyling
        {
            FontGroup = group,
            Size = TextSizeConstants.Normal
        };
        CachedSettings.font = group.Id;
    }

    private void UpdateOnlineSettings(Settings settings)
    {
        SettingsClient.UpdateSettings(settings.ToDto());
    }
    
    /// <summary>
    /// Update the active text size used in the website.
    /// </summary>
    /// <param name="size">Font size to change to.</param>
    private void UpdateTextSize(TextSizeEnum size)
    {
        switch (size)
        {
            case TextSizeEnum.Small:
                _theme.TextStyling.Size = TextSizeConstants.Small;
                _theme.UpdateTheme();
                activeTextStyling = new TextStyling
                {
                    FontGroup = _theme.TextStyling.FontGroup,
                    Size = TextSizeConstants.Small
                };
                CachedSettings.fontsize = 1;
                break;
            case TextSizeEnum.Normal:
                _theme.TextStyling.Size = TextSizeConstants.Normal;
                _theme.UpdateTheme();
                activeTextStyling = new TextStyling
                {
                    FontGroup = _theme.TextStyling.FontGroup,
                    Size = TextSizeConstants.Normal
                };
                CachedSettings.fontsize = 2;
                break;
            case TextSizeEnum.Large:
                _theme.TextStyling.Size = TextSizeConstants.Large;
                _theme.UpdateTheme();
                activeTextStyling = new TextStyling
                {
                    FontGroup = _theme.TextStyling.FontGroup,
                    Size = TextSizeConstants.Large
                };
                CachedSettings.fontsize = 3;
                break;
            case TextSizeEnum.VerySmall:
                _theme.TextStyling.Size = TextSizeConstants.VerySmall;
                _theme.UpdateTheme();
                activeTextStyling = new TextStyling
                {
                    FontGroup = _theme.TextStyling.FontGroup,
                    Size = TextSizeConstants.VerySmall
                };
                CachedSettings.fontsize = 0;
                break;
            case TextSizeEnum.VeryLarge:
                _theme.TextStyling.Size = TextSizeConstants.VeryLarge;
                _theme.UpdateTheme();
                activeTextStyling = new TextStyling
                {
                    FontGroup = _theme.TextStyling.FontGroup,
                    Size = TextSizeConstants.VeryLarge
                };
                CachedSettings.fontsize = 4;
                break;
        }

        Console.WriteLine("Update cached settings font size to " + CachedSettings.fontsize + "");
    }
}
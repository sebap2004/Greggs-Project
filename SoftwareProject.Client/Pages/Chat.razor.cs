using System.Net.Http.Json;
using System.Security.Claims;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using MudBlazor;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Themes;
using SoftwareProject.Services;
using Microsoft.Data.SqlClient;

namespace SoftwareProject.Client.Pages;

public partial class Chat : ComponentBase
{
    private List<Message> apiResponses = new();
    private bool UseFake { get; set; }

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
    
    
    
    private Topic topic = new Topic();
    private Message message = new Message();
    // TODO: REMOVE THIS WHEN WORKING CORRECTLY
    bool TOPICTEST = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _theme = new DefaultTheme();

        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (!user.Identity!.IsAuthenticated)
        {
            // NavigationManager.NavigateTo($"/access-denied/{Uri.EscapeDataString("notauthorized")}");
        }
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
        if (Question == "") return;
        string tempQuestion = (SummariseText ? "SUMMARISE THIS TEXT: " : "") + Question;
        // var translation = await http.PostAsJsonAsync("api/translate", new TranslateRequest
        //     {
        //         Text = tempQuestion,
        //         Language = Language.Code
        //     }
        // );
        // var result = await translation.Content.ReadAsStringAsync();
        
        apiResponses.Add(new Message
        {
            content = tempQuestion,
            isUser = true
        });
        SendingDisabled = true;
        Question = "";
        string response = await ai.GetMessage(tempQuestion, UseFake);
        apiResponses.Add(new Message
        {
            content = response,
            isUser = false
        });
        Question = "";
        SendingDisabled = false;

        if (TOPICTEST == false)
        {
            UploadTopic(tempQuestion);
        }
        
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
        Console.WriteLine(check.User);
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
            Snackbar.Add("You have been logged out.", Severity.Success);
            NavigationManager.NavigateTo("/");
        }
    }
}

class Message
{
    public string content { get; set; }
    public bool isUser { get; set; }
}



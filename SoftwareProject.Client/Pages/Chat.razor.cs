using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;
using MudBlazor;
using SoftwareProject.Themes;

namespace SoftwareProject.Client.Pages;

public partial class Chat : ComponentBase
{
    private List<Message> apiResponses = new();
    private bool UseFake { get; set; }

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
        string tempQuestion = Question;
        apiResponses.Add(new Message
        {
            content = tempQuestion,
            isUser = true
        });
        Question = "";
        SendingDisabled = true;
        string response = await ai.GetMessage(tempQuestion, UseFake);
        apiResponses.Add(new Message
        {
            content = response,
            isUser = false
        });
        SendingDisabled = false;
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
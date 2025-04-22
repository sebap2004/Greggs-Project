using Microsoft.AspNetCore.Components;
using MudBlazor;
using SoftwareProject.Themes;

namespace SoftwareProject.Components.Pages;

public partial class Chat : ComponentBase
{
    private bool _drawerOpen = true;
    private bool _isDarkMode = true;
    
    private MudTheme? _theme = null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _theme = new DefaultTheme();
        
        
    }

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void DarkModeToggle()
    {
        _isDarkMode = !_isDarkMode;
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
using MudBlazor;
using Microsoft.AspNetCore.Components;
using SoftwareProject.Themes;

namespace SoftwareProject.Components.Layout;
public partial class MainLayout : LayoutComponentBase
{
    private bool _drawerOpen = true;
    private bool _isDarkMode = true;
    private MudTheme? _theme = null;

    protected override void OnInitialized()
    {
        base.OnInitialized();

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
}
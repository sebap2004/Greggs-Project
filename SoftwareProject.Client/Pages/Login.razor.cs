using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using MudBlazor;
using SoftwareProject.Client.Models;
using SoftwareProject.Client.Providers;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Pages;

/// <summary>
/// Login page code-behind.
/// </summary>
public partial class Login : ComponentBase
{
    private Account account = new Account();
    private RegisterModel registerModel;
    private LoginModel loginModel;
    private bool AttemptingLogin = false;

    /// <summary>
    /// Instantiate a new RegisterModel and LoginModel class for account creation and login.
    /// This method is created because field variables in the component are created before the class injection.
    /// Whereas the method is run after meaning the parameters cannot be passed in a field variable.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        registerModel = new RegisterModel(account, http);
        loginModel = new LoginModel(account, http);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Attempt to log in to the website.
    /// </summary>
    private async Task AttemptLogin()
    {
        AttemptingLogin = true;
        var loginAttempt = await loginModel.LoginSubmit();
        
        if (loginAttempt == LoginStatus.Success)
        {
            if (AuthStateProvider is CookieAuthStateProvider customProvider)
            {
                await customProvider.NotifyAuthenticationStateChanged();
                snackbar.Add("Login Successful!", Severity.Success);
                navigationManager.NavigateTo("/chat");
                StateHasChanged();
            }
        }
        else
        {
            snackbar.Add("Login Failed", Severity.Error);
        }
        AttemptingLogin = false;
    }
    
    /// <summary>
    /// Attempt to create an account for the website.
    /// </summary>
    private async Task AttemptRegister()
    {
        AttemptingLogin = true;
        var registerAttempt = await registerModel.RegisterAccount();
        if (registerAttempt == RegisterStatus.Success)
        {
            if (AuthStateProvider is CookieAuthStateProvider customProvider)
            {
                await customProvider.NotifyAuthenticationStateChanged();
                snackbar.Add("Register Successful!", Severity.Success);
                navigationManager.NavigateTo("/chat");
            }
        }
        else
        {
            snackbar.Add("Login Failed", Severity.Error);
        }
        AttemptingLogin = false;
    }
}
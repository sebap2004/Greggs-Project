using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;
using SoftwareProject.Services;

namespace SoftwareProject.Components.Pages;

public partial class Login : ComponentBase
{
    private Account account = new Account();
    private RegisterModel registerModel;
    private LoginModel loginModel;

    /// <summary>
    /// Instantiate a new RegisterModel and LoginModel class for account creation and login.
    /// This method is created because field variables in the component are created before the class injection.
    /// Whereas the method is run after meaning the parameters cannot be passed in a field variable.
    /// </summary>
    protected override void OnInitialized()
    {
        registerModel = new RegisterModel(account, navigationManager, http);
        loginModel = new LoginModel(account, navigationManager, http);
    }

    private async Task AttemptLogin()
    {
        var loginAttempt = await loginModel.LoginSubmit();
        if (loginAttempt == LoginStatus.Success)
        {
            snackbar.Add("Login Successful!", Severity.Success);
            navigationManager.NavigateTo("/chat");
        }
        else
        {
            snackbar.Add("Login Failed", Severity.Error);
        }
    }
    
    private async Task AttemptRegister(EditContext editContext)
    {
        var registerAttempt = await registerModel.RegisterAccount(editContext);
        if (registerAttempt == RegisterStatus.Success)
        {
            snackbar.Add("Register Successful!", Severity.Success);
            navigationManager.NavigateTo("/chat");
        }
        else
        {
            snackbar.Add("Login Failed", Severity.Error);
        }
    }
}

public class RegisterModel
{
    // CLASS VARIABLES
    private Account account;
    private NavigationManager navigationManager;
    private HttpClient httpClient;

    public string firstPassword = "";
    public string secondPassword = "";
    
    /// <summary>
    /// CONSTRUCTOR
    /// </summary>
    /// <param name="pAccount">Stores the account table</param>
    /// <param name="pAccountService">Stores the AccountService class</param>
    public RegisterModel(Account pAccount, NavigationManager pNavigationManager, HttpClient pClient)
    {
        httpClient = pClient;
        account = pAccount;
        navigationManager = pNavigationManager;
    }

    /// <summary>
    /// Takes the information a user input in the form and calls the CRUD operation for the account table.
    /// </summary>
    /// <param name="editContext">Tracks the changes made in the form</param>
    /// <param name="httpClient">HTTP client used for making requests to the authentication api</param>
    public async Task<RegisterStatus> RegisterAccount(EditContext editContext)
    {
        // Validates password.
        if (firstPassword == secondPassword)
        {
            account.password = firstPassword;
            
            // Insert into table.
            try
            {
                var newAccount = (Account)editContext.Model;
                newAccount.account_id = 0;
                newAccount.role = "Fuck you";
                Console.WriteLine("Starting Register Process.");
                var jsonContent = System.Text.Json.JsonSerializer.Serialize(newAccount.AccountModel);
                Console.WriteLine($"Request body: {jsonContent}");

                var registerAttempt = await httpClient.PostAsJsonAsync("api/authentication/register", newAccount.AccountModel);
                if (registerAttempt.IsSuccessStatusCode)
                {
                    return RegisterStatus.Success;
                }
                else
                {
                    Console.WriteLine(registerAttempt.StatusCode);
                    Console.WriteLine(registerAttempt.ReasonPhrase);
                    Console.WriteLine(registerAttempt.RequestMessage);
                    var content = await registerAttempt.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    return RegisterStatus.Failure;
                }
            }
            catch (SqlException e)
            {
                await Console.Error.WriteLineAsync($"Error Connecting to Database: \n{e.Message}"); 
                return RegisterStatus.Failure;
            }
        }

        await Console.Error.WriteLineAsync("Passwords do not match");
        return RegisterStatus.Failure;
    }
}

public class LoginModel
{
    // CLASS VARIABLES
    private Account account;
    private NavigationManager navigationManager;
    private HttpClient httpClient;
    
    /// <summary>
    /// CONSTRUCTOR
    /// </summary>
    /// <param name="pAccount">Stores the account table</param>
    /// <param name="pAccountService">Stores the AccountService class</param>
    /// <param name="pNavigationManager">Stores the Navigation Manager</param>
    public LoginModel(Account pAccount, NavigationManager pNavigationManager, HttpClient pClient)
    {
        httpClient = pClient;
        account = pAccount;
        navigationManager = pNavigationManager;
    }
    
    /// <summary>
    /// Takes the information a user input in the form and checks the account table for a valid login.
    /// <param name="httpClient">HTTP client used for making requests to the authentication api</param>
    /// </summary>
    public async Task<LoginStatus> LoginSubmit()
    {
        try
        {
            Console.WriteLine("Starting Login Process.");
            
            // These lines are required, otherwise the httpclient won't send the request.
            // TODO: Consider making login and register models for specific purposes
            account.role = "login";
            account.username = "login";
            
            // Sends a post request to the authentication api.
            var checkAccount = await httpClient.PostAsJsonAsync("api/authentication/login", account.AccountModel);
            if (checkAccount.IsSuccessStatusCode)
            {
                // Redirect to <webpage> if an account is found.
                return LoginStatus.Success;
            }

            Console.WriteLine(checkAccount.StatusCode);
            Console.WriteLine(checkAccount.ReasonPhrase);
            Console.WriteLine(checkAccount.RequestMessage);
            var content = await checkAccount.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            return LoginStatus.Failure;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Error Connecting to Database: \n{e.Message}");
            return LoginStatus.Failure;
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        registerModel = new RegisterModel(account, accountService, navigationManager);
        loginModel = new LoginModel(account, accountService, navigationManager);
    }
}

public class RegisterModel
{
    // CLASS VARIABLES
    private Account account;
    private IAccountService accountService;
    private NavigationManager navigationManager;

    public string firstPassword = "";
    public string secondPassword = "";
    
    /// <summary>
    /// CONSTRUCTOR
    /// </summary>
    /// <param name="pAccount">Stores the account table</param>
    /// <param name="pAccountService">Stores the AccountService class</param>
    public RegisterModel(Account pAccount, IAccountService pAccountService, NavigationManager pNavigationManager)
    {
        account = pAccount;
        accountService = pAccountService;
        navigationManager = pNavigationManager;
    }
    
    /// <summary>
    /// Takes the information a user input in the form and calls the CRUD operation for the account table.
    /// </summary>
    /// <param name="editContext">Tracks the changes made in the form</param>
    public async Task RegisterAccount(EditContext editContext)
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
                await accountService.CreateAccount(newAccount);
                navigationManager.NavigateTo("/login");
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error Connecting to Database: \n{e.Message}");
            }
        }
        else
        {
            Console.WriteLine("Passwords do not match");
        }
    }
}

public class LoginModel
{
    // CLASS VARIABLES
    private Account account;
    private IAccountService accountService;
    private NavigationManager navigationManager;
    
    /// <summary>
    /// CONSTRUCTOR
    /// </summary>
    /// <param name="pAccount">Stores the account table</param>
    /// <param name="pAccountService">Stores the AccountService class</param>
    /// <param name="pNavigationManager">Stores the Navigation Manager</param>
    public LoginModel(Account pAccount, IAccountService pAccountService, NavigationManager pNavigationManager)
    {
        account = pAccount;
        accountService = pAccountService;
        navigationManager = pNavigationManager;
    }
    
    /// <summary>
    /// Takes the information a user input in the form and checks the account table for a valid login.
    /// </summary>
    public async Task LoginSubmit()
    {
        try
        {
            // Redirect to <webpage> if an account is found.
            var checkAccount = await accountService.LoginAccount(account.email, account.password);
            if (checkAccount != null)
            {
                navigationManager.NavigateTo("/");
            }
            else
            {
                Console.WriteLine("Invalid email or password");
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Error Connecting to Database: \n{e.Message}");
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoftwareProject.Data;
using SoftwareProject.Services;

namespace SoftwareProject.Components.Pages;

public partial class Login : ComponentBase
{
    private Account account = new Account();
    
    private LoginModel loginModel = new();
    private RegisterModel registerModel = new();
    
    /// <summary>
    /// Takes the information a user input in the form and calls the CRUD operation for the account table.
    /// </summary>
    /// <param name="editContext">Tracks the changes made in the form</param>
    public void registerAccount(EditContext editContext)
    {
        var newAccount = (Account)editContext.Model;
        newAccount.account_id = 0;
        accountService.CreateAccount(newAccount);
    }
}

public class LoginModel
{
    
}

public class RegisterModel
{
    
}
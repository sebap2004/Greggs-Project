using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SoftwareProject.Data;
using SoftwareProject.Services;

namespace SoftwareProject.Components.Pages;

public partial class Login : ComponentBase
{
    private Account account = new Account();
    
    // private LoginModel loginModel = new();
    // private RegisterModel registerModel = new();
    
    /// <summary>
    /// Takes the information a user input in the form and calls the CRUD operation for the account table.
    /// </summary>
    /// <param name="editContext">Tracks the changes made in the form</param>
    public async Task registerAccount(EditContext editContext)
    {
        try
        {
            var newAccount = (Account)editContext.Model;
            newAccount.account_id = 0;
            await accountService.CreateAccount(newAccount);
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Error Connecting to Database: \n{e.Message}");
        }
    }
    
    /// <summary>
    /// Takes the information a user input in the form and checks the account table for a valid login.
    /// </summary>
    private async Task LoginSubmit()
    {
        try
        {
            var checkAccount = await accountService.LoginAccount(account.email, account.password);

            if (checkAccount != null)
            {
                navigationManager.NavigateTo("/");
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Error Connecting to Database: \n{e.Message}");
        }
    }
}

public class LoginModel
{

}

public class RegisterModel
{
    
}
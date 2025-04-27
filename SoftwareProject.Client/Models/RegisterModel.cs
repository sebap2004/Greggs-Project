using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Models;

public class RegisterModel
{
    // CLASS VARIABLES
    private Account account;
    private HttpClient httpClient;

    [Required]
    public string username { get; set; }    
    
    [Required]
    [EmailAddress]
    public string email { get; set; }
    
    [Required]
    [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
    public string firstPassword { get; set; }
    
    [Required]
    [Compare(nameof(firstPassword))]
    public string secondPassword { get; set; }

    /// <summary>
    /// CONSTRUCTOR
    /// </summary>
    /// <param name="pAccount">Stores the account table</param>
    /// <param name="pClient">Reference to httpClient for making API requests.</param>
    public RegisterModel(Account pAccount, HttpClient pClient)
    {
        httpClient = pClient;
        account = pAccount;
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
                Console.WriteLine("Starting Register Process.");
                var jsonContent = System.Text.Json.JsonSerializer.Serialize(newAccount.AccountModel);
                Console.WriteLine($"Request body: {jsonContent}");

                var registerAttempt = await httpClient.PostAsJsonAsync("api/authentication/register", newAccount.AccountModel);
                if (registerAttempt.IsSuccessStatusCode)
                {
                    return RegisterStatus.Success;
                }
                Console.WriteLine(registerAttempt.StatusCode);
                Console.WriteLine(registerAttempt.ReasonPhrase);
                Console.WriteLine(registerAttempt.RequestMessage);
                var content = await registerAttempt.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                return RegisterStatus.FailureToCreateAccount;
            }
            catch (SqlException e)
            {
                await Console.Error.WriteLineAsync($"Error Connecting to Database: \n{e.Message}"); 
                return RegisterStatus.FailureDatabaseError;
            }
        }

        await Console.Error.WriteLineAsync("Passwords do not match");
        return RegisterStatus.FailurePasswordsDontMatch;
    }
}
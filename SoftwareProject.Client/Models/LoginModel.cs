using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Microsoft.Data.SqlClient;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Models;

/// <summary>
/// Login Model used in the Login page.
/// </summary>
public class LoginModel
{
    // CLASS VARIABLES
    /// <summary>
    /// Account to be created
    /// </summary>
    private Account account;
    
    /// <summary>
    /// HTTP client to be used in the request
    /// </summary>
    private HttpClient httpClient;

    /// <summary>
    /// Email property bound to the EditForm.
    /// Attributes are validation properties.
    /// </summary>
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress]
    public string email { get; set; }
    
    
    /// <summary>
    /// Password property bound to the EditForm.
    /// Attributes are validation properties.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string password { get; set; }  

    /// <summary>
    /// CONSTRUCTOR
    /// </summary>
    /// <param name="pAccount">Stores the account table</param>
    /// /// <param name="pClient">Reference to httpClient for making API requests.</param>
    public LoginModel(Account pAccount, HttpClient pClient)
    {
        httpClient = pClient;
        account = pAccount;
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
            account = new()
            {
                username = "login",
                email = email,
                password = password,
            };
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(account.AccountModel());
            Console.WriteLine($"Request body: {jsonContent}");
            // Sends a post request to the authentication api. 
            var checkAccount = await httpClient.PostAsJsonAsync("api/authentication/login", account.AccountModel());

            if (checkAccount.IsSuccessStatusCode)
            {
                // Redirect to <webpage> if an account is found.
                return LoginStatus.Success;
            }

            // Detailed Error Logging.
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
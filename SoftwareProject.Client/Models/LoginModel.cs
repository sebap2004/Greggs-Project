using System.Net.Http.Json;
using Microsoft.Data.SqlClient;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Models;

public class LoginModel
{
    // CLASS VARIABLES
    private Account account;
    private HttpClient httpClient;

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
            // TODO: Consider making login and register models for specific purposes
            account.username = "login";

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(account.AccountModel);
            Console.WriteLine($"Request body: {jsonContent}");
            // Sends a post request to the authentication api. 
            var checkAccount = await httpClient.PostAsJsonAsync("api/authentication/login", account.AccountModel);

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
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoftwareProject.Data;

/// <summary>
/// Account contains references to the Account table in the database.
/// Use this to access the columns on the Account table.
/// </summary>
public class Account
{
    [Key]
    public int account_id { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}

/// <summary>
/// Model to use in data transfer with HTTP requests.
/// </summary>
[Serializable]
public class AccountModel
{
    public AccountModel()
    {
        
    }
    public AccountModel(int accountID, string username, string email, string password)
    {
        AccountID = accountID;
        Username = username;
        Email = email;
        Password = password;
    }
    public int AccountID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

/// <summary>
/// Extension class to allow conversion between account data transfer objects and account database objects.
/// </summary>
public static class AccountExtensions
{
    /// <summary>
    /// Converts an account model to an account database object
    /// </summary>
    /// <param name="accountModel">Account model to convert</param>
    /// <returns></returns>
    public static Account ToAccount(this AccountModel accountModel)
    {
        return new Account
        {
            account_id = accountModel.AccountID,
            username = accountModel.Username,
            email = accountModel.Email,
            password = accountModel.Password,
        };
    }

    /// <summary>
    /// Converts an account database object to an account model object.
    /// </summary>
    /// <param name="account">Database object</param>
    /// <returns></returns>
    public static AccountModel AccountModel(this Account account)
    {
        return new AccountModel
        {
            AccountID = account.account_id,
            Username = account.username,
            Email = account.email,
            Password = account.password,
        };   
    }
}
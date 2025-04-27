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

public static class AccountExtensions
{
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
using System.ComponentModel.DataAnnotations;

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
    
    public AccountModel AccountModel => new()
    {
        AccountID = account_id,
        Username = username,
        Email = email,
        Password = password,
    };
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
    
    public Account ToAccount()
    {
        return new Account
        {
            account_id = AccountID,
            username = Username,
            email = Email,
            password = Password,
        };
    }
}
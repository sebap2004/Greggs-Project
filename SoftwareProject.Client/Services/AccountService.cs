using Microsoft.EntityFrameworkCore;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;
using BCrypt.Net;

namespace SoftwareProject.Services;

/// <summary>
/// AccountService contains the operations for the account table.
/// Use this for CRUD operations on the account table.
/// </summary>
public class AccountService : IAccountService 
{
    // CLASS VARIABLES
    // Assign DbContext
    private IDbContextFactory<ChatbotDbContext> dbContextFactory;

    /// <summary>
    /// CONSTRUCTOR
    /// Assigns the Chatbot DbContext so that the database can be accessed.
    /// </summary>
    /// <param name="pDbContextFactory">Stores the DbContext class</param>
    public AccountService(IDbContextFactory<ChatbotDbContext> pDbContextFactory)
    {
        dbContextFactory = pDbContextFactory;
    }

    /// <summary>
    /// Adds a user created accounts to the database.
    /// </summary>
    /// <param name="account">Stores the account table</param>
    public async Task<CreateAccountResultDto> CreateAccount(Account account)
    {
        Console.WriteLine("Starting Account Creation Process.");
        await using var context = await dbContextFactory.CreateDbContextAsync();
        
        // Checks if an account exists already
        Console.WriteLine("Checking if account exists already.");
        var existingAccount = await context.Account.FirstOrDefaultAsync(a => a.email == account.email);
        if (existingAccount != null)
        {
            Console.WriteLine("Account already exists.");
            return new CreateAccountResultDto(RegisterStatus.FailureAccountExists, 0);
        }
        
        await context.Account.AddAsync(account);
        await context.SaveChangesAsync();

        Console.WriteLine("Checking if account was created.");
        var createdAccount = await context.Account.FirstOrDefaultAsync(a => a.email == account.email);
        if (createdAccount == null)
        {
            Console.WriteLine("Account was not created.");
            return new CreateAccountResultDto(RegisterStatus.FailureToCreateAccount, 0);
        }
        
        return new CreateAccountResultDto(RegisterStatus.Success, createdAccount.account_id);;
    }
    
    /// <summary>
    /// Checks the database for a valid account to log into.
    /// </summary>
    /// <param name="email">Stores user input Email</param>
    /// <param name="password">Stores user input password</param>
    /// <returns>Logged in Account</returns>
    public async Task<Account?> LoginAccount(string email, string password)
    {
        Console.WriteLine("Attempting to login with email: " + email + " and password: " + password);
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        Console.WriteLine("Hased password: " + hashedPassword);
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var account = await context.Account.FirstOrDefaultAsync(a => a.email == email);
        if (BCrypt.Net.BCrypt.Verify(password, account?.password))
        {
            return account;
        }
        return null;
    }
}
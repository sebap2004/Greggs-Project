using Microsoft.EntityFrameworkCore;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

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
    public async Task<RegisterStatus> CreateAccount(Account account)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        
        // Checks if an account exists already
        var existingAccount = await context.Account.FirstOrDefaultAsync(a => a.email == account.email);
        if (existingAccount != null)
            return RegisterStatus.Failure;
            
        // Checks account has been created
        var createdAccount = await context.Account.FirstOrDefaultAsync(a => a.email == account.email);
        if (createdAccount == null)
            return RegisterStatus.Failure;
            
        await context.Account.AddAsync(account);
        await context.SaveChangesAsync();

        return RegisterStatus.Success;
    }
    
    /// <summary>
    /// Checks the database for a valid account to log into.
    /// </summary>
    /// <param name="email">Stores user input Email</param>
    /// <param name="password">Stores user input password</param>
    /// <returns></returns>
    public async Task<Account?> LoginAccount(string email, string password)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Account.FirstOrDefaultAsync(a => a.email == email && a.password == password);
    }
}
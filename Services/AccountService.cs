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
    public async Task CreateAccount(Account account)
    {
        using (var context = dbContextFactory.CreateDbContext())
        {
            await context.Account.AddAsync(account);
            await context.SaveChangesAsync();
        }
    }
    
    /// <summary>
    /// Checks the database for a valid account to log into.
    /// </summary>
    /// <param name="email">Stores user input Email</param>
    /// <param name="password">Stores user input password</param>
    /// <returns></returns>
    public async Task<Account?> LoginAccount(string email, string password)
    {
        using (var context = dbContextFactory.CreateDbContext())
        {
            return await context.Account.FirstOrDefaultAsync(a => a.email == email && a.password == password);
        }
    }
}
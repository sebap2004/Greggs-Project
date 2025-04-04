using Microsoft.EntityFrameworkCore;
using SoftwareProject.Data;

namespace SoftwareProject.Services;
/// <summary>
/// AccountService contains the operations for the account table.
/// Use this for CRUD operations on the account table.
/// </summary>
public class AccountService
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
    public void CreateAccount(Account account)
    {
        using (var context = dbContextFactory.CreateDbContext())
        {
            context.Account.Add(account);
            context.SaveChanges();
        }
    }
    
    
}
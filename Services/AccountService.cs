using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
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
    private IHttpContextAccessor httpContextAccessor;
    

    /// <summary>
    /// CONSTRUCTOR
    /// Assigns the Chatbot DbContext so that the database can be accessed.
    /// </summary>
    /// <param name="pDbContextFactory">Stores the DbContext class</param>
    public AccountService(IDbContextFactory<ChatbotDbContext> pDbContextFactory, IHttpContextAccessor pHttpContextAccessor)
    {
        dbContextFactory = pDbContextFactory;
        httpContextAccessor = pHttpContextAccessor;
    }

    /// <summary>
    /// Adds user-created accounts to the database.
    /// </summary>
    /// <param name="account">Stores the account table</param>
    public async Task<RegisterStatus> CreateAccount(Account account)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        try
        {
            await context.Account.AddAsync(account);
            await context.SaveChangesAsync();
            var loggedInAccount = await context.Account.FirstOrDefaultAsync(a => a.email == account.email && a.password == account.password);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, loggedInAccount.username),
                new(ClaimTypes.NameIdentifier, account.account_id.ToString()),
                new(ClaimTypes.Email, loggedInAccount.email),
            };
                
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            if (httpContextAccessor.HttpContext != null)
            {
                await httpContextAccessor.HttpContext.SignInAsync("Cookies", principal);
                Console.WriteLine("Signing in!");
                return RegisterStatus.Success;
            }

            return RegisterStatus.Failure;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return RegisterStatus.Failure;
        }
    }
    
    /// <summary>
    /// Checks the database for a valid account to log into.
    /// </summary>
    /// <param name="email">Stores user input Email</param>
    /// <param name="password">Stores user input password</param>
    /// <returns></returns>
    public async Task<LoginStatus> LoginAccount(string email, string password)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var account = await context.Account.FirstOrDefaultAsync(a => a.email == email && a.password == password);
        if (account != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, account.username),
                new(ClaimTypes.NameIdentifier, account.account_id.ToString()),
                new(ClaimTypes.Email, account.email),
            };
                
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            if (httpContextAccessor.HttpContext != null)
            {
                await httpContextAccessor.HttpContext.SignInAsync("Cookies", principal);
            }

            return LoginStatus.Success;
        }
        return LoginStatus.Failure;
    }
}


using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareProject.Components.Pages;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private IDbContextFactory<ChatbotDbContext> dbContextFactory;
    
    public AuthenticationController(IDbContextFactory<ChatbotDbContext> pdbContextFactory)
    {
        dbContextFactory = pdbContextFactory;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountModel pAccount)
    {
        Console.WriteLine("Starting login process.");
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var account = await context.Account.FirstOrDefaultAsync(a => a.email == pAccount.Email && a.password == pAccount.Password);
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

            await HttpContext.SignInAsync("Cookies", principal);

            return Ok();
        }
        return BadRequest();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AccountModel account)
    {
        if (account == null)
            return BadRequest("Account data is null");
        
        if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password))
            return BadRequest("Required fields are missing");

        Console.WriteLine($"Received registration request for: {account.Email}");
    
        await using var context = await dbContextFactory.CreateDbContextAsync();
        try
        {
            var existingAccount = await context.Account.FirstOrDefaultAsync(a => a.email == account.Email);
            if (existingAccount != null)
                return BadRequest("Email already registered");
            
            await context.Account.AddAsync(account.ToAccount());
            await context.SaveChangesAsync();
        
            var loggedInAccount = await context.Account.FirstOrDefaultAsync(a => a.email == account.Email);
            if (loggedInAccount == null)
                return BadRequest("Failed to create account");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, loggedInAccount.username),
                new(ClaimTypes.NameIdentifier, loggedInAccount.account_id.ToString()),
                new(ClaimTypes.Email, loggedInAccount.email),
            };
            
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            Console.WriteLine($"Registration successful for: {account.Email}");
            
            return Ok();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Registration error: {e.Message}");
            return BadRequest($"Registration failed: {e.Message}");
        }
    }

}

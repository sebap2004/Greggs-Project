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
        var account = await context.Account.FirstOrDefaultAsync(a => 
            a.email == pAccount.Email && a.password == pAccount.Password);
        
        if (account != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, account.username),
                new(ClaimTypes.NameIdentifier, account.account_id.ToString()),
                new(ClaimTypes.Email, account.email),
                new(ClaimTypes.Role, account.role ?? "User") // Add role claim
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            // Standard auth cookie creation
            await HttpContext.SignInAsync("Cookies", principal, authProperties);
            
            // Also manually set a visible cookie to check if cookies work at all
            Response.Cookies.Append("auth_visible", "true", new CookieOptions
            {
                HttpOnly = false, // This one should be visible to JavaScript
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            Console.WriteLine($"User signed in. Server time: {DateTime.UtcNow}");
            
            // Print response cookies for debugging
            Console.WriteLine("Cookies being set:");
            foreach (var cookie in Response.Headers.Where(h => h.Key == "Set-Cookie"))
            {
                Console.WriteLine($"Set-Cookie: {cookie.Value}");
            }
            
            return Ok(new
            {
                IsAuthenticated = true,
                Username = account.username,
                Email = account.email,
                Id = account.account_id
            });
        }

        return BadRequest("Invalid credentials");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AccountModel account)
    {
        if (account == null)
            return BadRequest("Account data is null");

        if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Username) ||
            string.IsNullOrEmpty(account.Password))
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
            
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync("Cookies", principal, authProperties);

            Console.WriteLine($"Registration successful for: {account.Email}");

            return Ok();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Registration error: {e.Message}");
            return BadRequest($"Registration failed: {e.Message}");
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }

        var claims = User.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToList();
        return Ok(claims);
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Ok();
    }
}
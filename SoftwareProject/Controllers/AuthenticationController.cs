using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Controllers;
/// <summary>
/// API Controller for authentication.
/// This controller handles the login, registration, getting and logging out of users.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// Factory to create DB context instances
    /// </summary>
    private IAccountService accountService;

    /// <summary>
    /// CONSTRUCTOR
    /// Assigns the Chatbot DbContext so that the database can be accessed.
    /// </summary>
    /// <param name="pDbContextFactory">Stores the DbContext class</param>
    public AuthenticationController(IAccountService pAccountService)
    {
        accountService = pAccountService;
    }

    /// <summary>
    /// Authenticates a user by verifying their credentials against the database and initiates a session with authentication cookies.
    /// </summary>
    /// <param name="pAccount">Account model that has been passed from the http request body</param>
    /// <returns>Result of action</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountModel pAccount)
    {
        Console.WriteLine("Starting login process.");

        var account = await accountService.LoginAccount(pAccount.Email, pAccount.Password);
        
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

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync("Cookies", principal, authProperties);

            Console.WriteLine($"User signed in. Server time: {DateTime.UtcNow}");
            
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

    
    /// <summary>
    /// Creates a new account in the database and initiates a session with authentication cookies.
    /// </summary>
    /// <param name="account">Account model that has been passed from the http request body</param>
    /// <returns>Result of action</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AccountModel account)
    {
        if (account == null)
            return BadRequest("Account data is null");

        if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Username) ||
            string.IsNullOrEmpty(account.Password))
            return BadRequest("Required fields are missing");

        Console.WriteLine($"Received registration request for: {account.Email}");
        
        try
        {
            var status = await accountService.CreateAccount(account.ToAccount());

            if (status == RegisterStatus.Success)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, account.Username),
                    new(ClaimTypes.NameIdentifier, account.AccountID.ToString()),
                    new(ClaimTypes.Email, account.Email),
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
            return BadRequest("Registration Failed: " + status);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Registration error: {e.Message}");
            return BadRequest($"Registration failed: {e.Message}");
        }
    }

    /// <summary>
    /// Gets the authentication status of the user.
    /// </summary>
    /// <returns>Result of action</returns>
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


    /// <summary>
    /// Logs out active user.
    /// </summary>
    /// <returns>Result of action</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Ok();
    }
}
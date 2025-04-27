using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace SoftwareProject.Client.Providers;

/// <summary>
/// Custom authentication state provider that checks cookies to verify authentication status. 
/// </summary>
public class CookieAuthStateProvider : AuthenticationStateProvider
{
    /// <summary>
    /// Http client used to make requests to the API controller
    /// </summary>
    private readonly HttpClient _httpClient;
    
    /// <summary>
    /// Cached authentication state that is returned when the authentication state is requested
    /// </summary>
    private AuthenticationState _cachedAuthState = new(new ClaimsPrincipal(new ClaimsIdentity()));

    /// <summary>
    /// Constructor for the CookieAuthStateProvider class.
    /// Http client included as a parameter for dependency injection.
    /// </summary>
    /// <param name="httpClient">Http client used to make requests about authentication status</param>
    public CookieAuthStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets authentication state from the API controller
    /// </summary>
    /// <returns>Authentication state of the user</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/authentication/user");
            if (response.IsSuccessStatusCode)
            {
                var claims = await response.Content.ReadFromJsonAsync<List<ClaimDto>>();
                if (claims != null && claims.Any())
                {
                    Console.WriteLine($"Found {claims.Count} claims from API");
                    var identity = new ClaimsIdentity(
                        claims.Select(c => new Claim(c.Type, c.Value)),
                        "cookie"
                    );
                    var principal = new ClaimsPrincipal(identity);
                    _cachedAuthState = new AuthenticationState(principal);
                    return _cachedAuthState;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
        }
        return _cachedAuthState;
    }

    /// <summary>
    /// Notifies the system that the authentication state has changed.
    /// </summary>
    public async Task NotifyAuthenticationStateChanged()
    {
        Console.WriteLine("Authentication state changed");
        _cachedAuthState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthState));
    }

}

/// <summary>
/// Data transfer object representing a claim with a type and a value.
/// </summary>
public class ClaimDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
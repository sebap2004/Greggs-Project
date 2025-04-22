using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
namespace SoftwareProject.Providers;

public class CookieAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private AuthenticationState _cachedAuthState = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public CookieAuthStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            Console.WriteLine("CookieAuthStateProvider: Getting authentication state");
            var response = await _httpClient.GetAsync("api/authentication/user");
            Console.WriteLine($"Auth state response: {response.StatusCode}");
            
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

    public void NotifyAuthenticationStateChanged()
    {
        Console.WriteLine("Authentication state change notified");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}


public class ClaimDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
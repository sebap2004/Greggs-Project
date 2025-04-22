using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
namespace SoftwareProject.Providers;

public class CookieAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public CookieAuthStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/authentication/user");
            if (response.IsSuccessStatusCode)
            {
                var claims = await response.Content.ReadFromJsonAsync<List<ClaimDto>>();
                var identity = new ClaimsIdentity(
                    claims?.Select(c => new Claim(c.Type, c.Value)),
                    "cookie"
                );
                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            }
        }
        catch
        {
            // ignored
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}


public class ClaimDto
{
    public string Type { get; set; }
    public string Value { get; set; }
}

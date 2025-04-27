using System.Net.Http.Json;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Client.Clients;

/// <summary>
/// Settings client to be used on the frontend. Shares the same interface as the settings service on the server.
/// </summary>
public class SettingsClient : ISettingsService
{
    
    /// <summary>
    /// HTTP client to make requests with.
    /// </summary>
    private readonly HttpClient _httpClient;
    
    /// <summary>
    /// Constructor for settings client. Injects HTTP client to make requests to the API controller
    /// </summary>
    /// <param name="httpClient">Client to be injected to</param>
    public SettingsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
    /// <summary>
    /// Gets settings using the HTTP client, with an account ID as the search index
    /// </summary>
    /// <param name="accountId">Account ID to search with</param>
    /// <returns>Settings gathered from the server</returns>
    public async Task<Settings> GetSettings(int accountId)
    {
        Settings settings = await _httpClient.GetFromJsonAsync<Settings>($"api/Settings/{accountId}") ?? new Settings();
        return settings;
    }

    /// <summary>
    /// Updates the settings for a user by sending the updated settings data with the HTTP client.
    /// </summary>
    /// <param name="settings">Data transfer object containing settings data to be updated.</param>
    /// <returns>Updated settings data from database</returns>
    public async Task<Settings> UpdateSettings(SettingsDto settings)
    {
        var updatedSettings = await _httpClient.PostAsJsonAsync("api/Settings/update", settings);
        return updatedSettings.IsSuccessStatusCode ? settings.ToSettings() : new Settings();
    }

    /// <summary>
    /// Creates new settings object using the HTTP client
    /// </summary>
    /// <param name="accountId">Account ID to create the settings for</param>
    /// <returns>Boolean representing success of the operation.</returns>
    public async Task<bool> CreateSettings(int accountId)
    {
        var response = await _httpClient.PutAsync($"api/Settings/create/{accountId}", null);
        return response.IsSuccessStatusCode;
    }
}
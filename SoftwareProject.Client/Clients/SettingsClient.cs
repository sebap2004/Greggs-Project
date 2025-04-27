using System.Net.Http.Json;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Client.Clients;

public class SettingsClient : ISettingsService
{
    private readonly HttpClient _httpClient;
    
    public SettingsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Settings> GetSettings(int accountId)
    {
        Settings settings = await _httpClient.GetFromJsonAsync<Settings>($"api/Settings/{accountId}") ?? new Settings();
        return settings;
    }

    public async Task<Settings> UpdateSettings(SettingsDto settings)
    {
        var updatedSettings = await _httpClient.PostAsJsonAsync("api/Settings/update", settings);
        return updatedSettings.IsSuccessStatusCode ? settings.ToSettings() : new Settings();
    }

    public async Task<bool> CreateSettings(int accountId)
    {
        var response = await _httpClient.PutAsync($"api/Settings/create/{accountId}", null);
        return response.IsSuccessStatusCode;
    }
}
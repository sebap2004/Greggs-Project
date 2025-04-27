using SoftwareProject.Client.Data;

namespace SoftwareProject.Client.Interfaces;

public interface ISettingsService
{
    public Task<Settings> GetSettings(int accountId);
    public Task<Settings> UpdateSettings(SettingsDto settings);
    public Task<bool> CreateSettings(int accountId);
}
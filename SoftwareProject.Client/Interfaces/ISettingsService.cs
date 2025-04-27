using SoftwareProject.Client.Data;

namespace SoftwareProject.Client.Interfaces;


/// <summary>
/// Interface for settings related operations.
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Gets settings searching with an account ID.
    /// </summary>
    /// <param name="accountId">Account ID to search with</param>
    /// <returns>Result of search. Settings object.</returns>
    public Task<Settings> GetSettings(int accountId);
    
    /// <summary>
    /// Updates settings using a data transfer object.
    /// </summary>
    /// <param name="settings">Object to update</param>
    /// <returns>Updated settings object.</returns>
    public Task<Settings> UpdateSettings(SettingsDto settings);
    
    /// <summary>
    /// Create settings.
    /// </summary>
    /// <param name="accountId">account ID to create the settings for.</param>
    /// <returns>Returns boolean representing success of the operation.</returns>
    public Task<bool> CreateSettings(int accountId);
}
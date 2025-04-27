using Microsoft.AspNetCore.Mvc;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Controllers;

/// <summary>
/// API controller handling settings.
/// Handles retrieval, creation and updating of settings.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private ISettingsService _settingsService;
    
    /// <summary>
    /// Constructor for settings controller
    /// </summary>
    /// <param name="pSettingsService">Settings service used for database operations</param>
    public SettingsController(ISettingsService pSettingsService)
    {
        _settingsService = pSettingsService;
    }
    
    
    /// <summary>
    /// Gets the settings of a user using their ID.
    /// </summary>
    /// <param name="userId">User ID to find the settings of.</param>
    /// <returns>Action result containing the settings object.</returns>
    [HttpGet("{userId}")]
    public async Task<ActionResult<Settings>> GetSettings(int userId)
    {
        return await _settingsService.GetSettings(userId);
    }

    
    /// <summary>
    /// Updates a user's settings.
    /// </summary>
    /// <param name="settings">Settings data transfer object to update in the database.</param>
    /// <returns>Updated settings from database.</returns>
    [HttpPost("update")]
    public async Task<ActionResult<Settings>> UpdateSettings(SettingsDto settings)
    {
        return await _settingsService.UpdateSettings(settings);
    }

    /// <summary>
    /// Creates a user's settings
    /// </summary>
    /// <param name="accountId">User's account ID to create a settings object for.</param>
    /// <returns>Action result containing boolean determining the success of the operation</returns>
    [HttpPut("create/{accountId}")]
    public async Task<ActionResult<bool>> CreateSettings(int accountId)
    {
        return await _settingsService.CreateSettings(accountId);
    }
}
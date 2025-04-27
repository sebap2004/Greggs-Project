using Microsoft.AspNetCore.Mvc;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private ISettingsService _settingsService;
    
    public SettingsController(ISettingsService pSettingsService)
    {
        _settingsService = pSettingsService;
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<Settings>> GetSettings(int userId)
    {
        return await _settingsService.GetSettings(userId);
    }

    [HttpPost("update")]
    public async Task<ActionResult<Settings>> UpdateSettings(SettingsDto settings)
    {
        return await _settingsService.UpdateSettings(settings);
    }

    [HttpPut("create/{accountId}")]
    public async Task<ActionResult<bool>> CreateSettings(int accountId)
    {
        return await _settingsService.CreateSettings(accountId);
    }
}
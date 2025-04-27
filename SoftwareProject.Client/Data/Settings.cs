using System.ComponentModel.DataAnnotations;

namespace SoftwareProject.Client.Data;

/// <summary>
/// Settings contains references to the Settings table in the database.
/// Use this to access the columns on the Settings table.
/// </summary>
public class Settings
{
    [Key]
    public int settings_id {get;set;}
    public bool darkmode {get;set;}
    public int fontsize {get;set;}
    public int font { get; set; }
    public string language {get;set;}
    public int account_id {get;set;}

    public SettingsDto ToDto()
    {
        return new SettingsDto
        {
            SettingsId = settings_id,
            DarkMode = darkmode,
            FontSize = fontsize,
            Font = font,
            Language = language,
            AccountId = account_id,
        };
    }
}

/// <summary>
/// Settings data transfer object to be used over HTTP requests.
/// </summary>
public class SettingsDto
{
    public int SettingsId { get; set; }
    public bool DarkMode { get; set; }
    public int FontSize { get; set; }
    public int Font { get; set; }
    public string Language { get; set; }
    public int AccountId { get; set; }
    
    public Settings ToSettings()
    {
        return new Settings
        {
            settings_id = SettingsId,
            darkmode = DarkMode,
            fontsize = FontSize,
            language = Language,
            account_id = AccountId,
        };
    }
}
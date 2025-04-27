using Microsoft.EntityFrameworkCore;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Client.SiteSettings;
using SoftwareProject.Data;

namespace SoftwareProject.Client.Services;

public class SettingsService : ISettingsService
{
    private IDbContextFactory<ChatbotDbContext> dbContextFactory;

    public SettingsService(IDbContextFactory<ChatbotDbContext> pDbContextFactory)
    {
        dbContextFactory = pDbContextFactory;
    }
    
    
    public async Task<Settings> GetSettings(int accountId)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Settings.FirstOrDefaultAsync(s => s.account_id == accountId) ?? new Settings();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Settings();
        }
    }

    public async Task<Settings> UpdateSettings(SettingsDto settings)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            var retrievedSettings = context.Settings.FirstOrDefault(s => s.account_id == settings.AccountId);
            retrievedSettings.darkmode = settings.DarkMode;
            retrievedSettings.language = settings.Language;
            retrievedSettings.fontsize = settings.FontSize;
            retrievedSettings.font = settings.Font;
            await context.SaveChangesAsync();
            return settings.ToSettings();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Settings();
        }
    }

    public async Task<bool> CreateSettings(int accountId)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            await context.Settings.AddAsync(new()
            {
                account_id = accountId,
                darkmode = false,
                language = Languages.English.Code,
                fontsize = 3,
            });
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}
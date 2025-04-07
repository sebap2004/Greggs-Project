using Microsoft.EntityFrameworkCore;

namespace SoftwareProject.Data;

/// <summary>
/// DbContext acts as the connection to the greggsproject database.
/// Allows the database to read and updated by the app.
/// </summary>
public class ChatbotDbContext : DbContext
{
    public ChatbotDbContext(DbContextOptions<ChatbotDbContext> options) : base(options)
    {
    }
    
    /// <summary>
    /// Calls the Account table from the greggsproject database.
    /// </summary>
    public DbSet<Account> Account { get; set; }
}
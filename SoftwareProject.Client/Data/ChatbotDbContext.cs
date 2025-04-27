using Microsoft.EntityFrameworkCore;
using SoftwareProject.Client.Data;

namespace SoftwareProject.Data;

/// <summary>
/// DbContext acts as the connection to the greggsproject database.
/// Allows the database to read and updated by the app.
/// </summary>
public class ChatbotDbContext : DbContext
{
    public ChatbotDbContext(DbContextOptions<ChatbotDbContext> options) : base(options) { }
    
    /// <summary>
    /// Represents account table
    /// </summary>
    public DbSet<Account> Account { get; set; }
    /// <summary>
    /// Represents topic table
    /// </summary>
    public DbSet<Topic> Topic { get; set; }
    /// <summary>
    /// Represents message table
    /// </summary>
    public DbSet<Message> Message { get; set; }
    /// <summary>
    /// Represents settings table
    /// </summary>
    public DbSet<Settings> Settings { get; set; }
}
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
    /// Calls and assigns the tables from the greggsproject database.
    /// </summary>
    public DbSet<Account> Account { get; set; }
    public DbSet<Topic> Topic { get; set; }
    public DbSet<Message> Message { get; set; }
}
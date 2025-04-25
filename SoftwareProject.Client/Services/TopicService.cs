using Microsoft.EntityFrameworkCore;
using SoftwareProject.Client.Data;
using SoftwareProject.Data;

namespace SoftwareProject.Services;

public class TopicService
{
    // CLASS VARIABLES
    // Assign DbContext
    private IDbContextFactory<ChatbotDbContext> dbContextFactory;
    
    /// <summary>
    /// CONSTRUCTOR
    /// Assigns the Chatbot DbContext so that the database can be accessed.
    /// </summary>
    /// <param name="pDbContextFactory">Stores the DbContext class</param>
    public TopicService(IDbContextFactory<ChatbotDbContext> pDbContextFactory)
    {
        dbContextFactory = pDbContextFactory;
    }
    
    /// <summary>
    /// Adds a topic to the database.
    /// </summary>
    /// <param name="topic">Stores the topic table</param>
    public async Task CreateTopic(Topic topic)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        await context.Topic.AddAsync(topic);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<Topic>> GetTopics(int accountId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Topic.Where(t => t.account_id == accountId).ToListAsync();
    }
}
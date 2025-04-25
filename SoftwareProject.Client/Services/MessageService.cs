using Microsoft.EntityFrameworkCore;
using SoftwareProject.Client.Data;
using SoftwareProject.Data;

namespace SoftwareProject.Services;

public class MessageService
{
    // CLASS VARIABLES
    // Assign DbContext
    private IDbContextFactory<ChatbotDbContext> dbContextFactory;
    
    /// <summary>
    /// CONSTRUCTOR
    /// Assigns the Chatbot DbContext so that the database can be accessed.
    /// </summary>
    /// <param name="pDbContextFactory">Stores the DbContext class</param>
    public MessageService(IDbContextFactory<ChatbotDbContext> pDbContextFactory)
    {
        dbContextFactory = pDbContextFactory;
    }
    
    /// <summary>
    /// Adds a topic to the database.
    /// </summary>
    /// <param name="topic">Stores the topic table</param>
    public async Task CreateMessage(Message message)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        await context.Message.AddAsync(message);
        await context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetMessages(int topicId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Message.Where(m => m.topic_id == topicId).ToListAsync();
    }
}
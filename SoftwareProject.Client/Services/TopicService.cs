using Microsoft.EntityFrameworkCore;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Data;

namespace SoftwareProject.Services;

public class TopicService : ITopicService
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
    public async Task<Topic> CreateTopic(Topic topic)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            var result = await context.Topic.AddAsync(topic);
            await context.SaveChangesAsync();
            Console.WriteLine("Topic created! ID: " + result.Entity.topic_id);
            topic.topic_id = result.Entity.topic_id;
            return topic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    /// <summary>
    /// Gets the topics of an account
    /// </summary>
    /// <param name="accountId">accountID to search topics by</param>
    /// <returns>List of the user's topics.</returns>
    public async Task<List<Topic>> GetTopics(int accountId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Topic.Where(t => t.account_id == accountId).ToListAsync();
    }

    /// <summary>
    /// Deletes a topic and all of its messages.
    /// </summary>
    /// <param name="topicId">ID of the topic to delete</param>
    /// <returns>Status of deletion.</returns>
    public async Task<TopicDeleteStatus> DeleteTopic(int topicId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var messages = context.Message.Where(m => m.topic_id == topicId);
        context.Message.RemoveRange(messages);
        var topic = context.Topic.FirstOrDefault(t => t.topic_id == topicId);
        if (topic != null) context.Topic.Remove(topic);
        await context.SaveChangesAsync();
        return TopicDeleteStatus.Success;
    }
}
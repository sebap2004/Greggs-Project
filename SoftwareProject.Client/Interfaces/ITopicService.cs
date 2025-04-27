using SoftwareProject.Client.Data;

namespace SoftwareProject.Client.Interfaces;


/// <summary>
/// Interface for topic related operations.
/// </summary>
public interface ITopicService
{
    /// <summary>
    /// Creates a topic.
    /// </summary>
    /// <param name="topic">Topic to create</param>
    /// <returns>Created topic entry on the database.</returns>
    Task<Topic> CreateTopic(Topic topic);
    
    /// <summary>
    /// Gets a list of topics using the account ID as a search index.
    /// </summary>
    /// <param name="accountId">Account ID to search with</param>
    /// <returns>Topics that belong to that account.</returns>
    Task<List<Topic>> GetTopics(int accountId);
    
    /// <summary>
    /// Deletes a topic using the topic ID as a search index.
    /// </summary>
    /// <param name="topicId">topic ID to search with</param>
    /// <returns>Deletion status of the topic</returns>
    Task<TopicDeleteStatus> DeleteTopic(int topicId);
}

/// <summary>
/// Enum representing the status of a topic deletion attempt.
/// </summary>
public enum TopicDeleteStatus
{
    Success,
    Failure
}
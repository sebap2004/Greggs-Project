using SoftwareProject.Client.Data;

namespace SoftwareProject.Client.Interfaces;

/// <summary>
/// Interface for message related methods
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Creates a message.
    /// </summary>
    /// <param name="message">Message data transfer object.</param>
    /// <returns>Status of the message creation attempt.</returns>
    Task<MessageSendStatus> CreateMessage(MessageDto message);
    
    /// <summary>
    /// Gets messages using a topic ID.
    /// </summary>
    /// <param name="topicId">Topic ID to search with</param>
    /// <returns>List of message data transfer objects.</returns>
    Task<List<MessageDto>> GetMessages(int topicId);
}

/// <summary>
/// Enum representing send status of a message attempt.
/// </summary>
public enum MessageSendStatus
{
    Success,
    Failure
}
using SoftwareProject.Client.Data;

namespace SoftwareProject.Client.Interfaces;

public interface IMessageService
{
    Task<MessageSendStatus> CreateMessage(MessageDto message);
    Task<List<MessageDto>> GetMessages(int topicId);
}

public enum MessageSendStatus
{
    Success,
    Failure
}
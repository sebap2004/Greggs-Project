using SoftwareProject.Client.Data;

namespace SoftwareProject.Client.Interfaces;

public interface ITopicService
{
    Task<Topic> CreateTopic(Topic topic);
    Task<List<Topic>> GetTopics(int accountId);
}

public enum TopicCreateStatus
{
    Success,
    Failure
}
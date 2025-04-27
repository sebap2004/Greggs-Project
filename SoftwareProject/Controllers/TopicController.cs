using Microsoft.AspNetCore.Mvc;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Services;

namespace SoftwareProject.Controllers;


/// <summary>
/// API controller for topics.
/// Handles creation, retrieval and deletion of topics
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    /// <summary>
    /// Constructor for topic controller.
    /// </summary>
    /// <param name="topicService">Topic service used for database operations.</param>
    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }
    
    /// <summary>
    /// Get topics from database using user ID as search index.
    /// </summary>
    /// <param name="userId">User ID to search</param>
    /// <returns>Action result containing the list of topics.</returns>
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Topic>>> GetTopics(int userId)
    {
        return await _topicService.GetTopics(userId);
    }

    /// <summary>
    /// Creates a topic in the database.
    /// </summary>
    /// <param name="topic">Topic to create</param>
    /// <returns>Created topic in the database</returns>
    [HttpPost]
    public async Task<ActionResult<Topic>> CreateTopic(Topic topic)
    {
        return await _topicService.CreateTopic(topic);
    }
    
    
    /// <summary>
    /// Deletes topic from database using topic ID as search index.
    /// </summary>
    /// <param name="topicId">Topic ID to search by</param>
    /// <returns>Action Result containing the status of the deletion.</returns>
    [HttpDelete("{topicId}")]
    public async Task<ActionResult<TopicDeleteStatus>> DeleteTopic(int topicId)
    {
        return await _topicService.DeleteTopic(topicId);
    }
}

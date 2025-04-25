using Microsoft.AspNetCore.Mvc;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Services;

namespace SoftwareProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Topic>>> GetTopics(int userId)
    {
        return await _topicService.GetTopics(userId);
    }

    [HttpPost]
    public async Task<ActionResult<Topic>> CreateTopic(Topic topic)
    {
        return await _topicService.CreateTopic(topic);
    }
}

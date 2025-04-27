using Microsoft.AspNetCore.Mvc;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;
namespace SoftwareProject.Controllers;

/// <summary>
/// API controller for messages.
/// This controller handles the retrieval and sending of messages.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private IMessageService messageService;
    
    /// <summary>
    /// Constructor for the Message Controller.
    /// </summary>
    /// <param name="pMessageService">Message service used for database operations</param>
    public MessageController(IMessageService pMessageService)
    {
        messageService = pMessageService;
    }
    
    
    /// <summary>
    /// Uses the message service to get a list of messages from a topic ID.
    /// </summary>
    /// <param name="topicId">Topic ID to search by</param>
    /// <returns>An action result containing the list of message data transfer objects</returns>
    [HttpGet("{topicId}")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(int topicId)
    { 
        return await messageService.GetMessages(topicId);
    }


    /// <summary>
    /// Uses the message service to create a new message in the database.
    /// </summary>
    /// <param name="message">Message DTO to add into the database.</param>
    /// <returns>Action result of the message creation</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageDto message)
    {
        try
        {
            await messageService.CreateMessage(message);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }
    }
}
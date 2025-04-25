using Microsoft.AspNetCore.Mvc;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private IMessageService messageService;
    
    public MessageController(IMessageService pMessageService)
    {
        messageService = pMessageService;
    }
    
    [HttpGet("{topicId}")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(int topicId)
    { 
        return await messageService.GetMessages(topicId);
    }

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
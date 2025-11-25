using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSocketsChatStudy.DTOs;
using WebSocketsChatStudy.Services.Interfaces;

namespace WebSocketsChatStudy.Controllers;

[Route("[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService) => _chatService = chatService;

    [HttpGet]
    [Authorize]
    [Route("GetUserRecipientByEmail")]
    public async Task<ActionResult<UserRecipientDTO>> GetUserRecepientByEmailAsync([FromQuery] string email)
    {
        try
        {
            var userDto = await _chatService.GetUserRecipientByEmailAsync(email);

            if (userDto == null)
                return NotFound();

            return Ok(userDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

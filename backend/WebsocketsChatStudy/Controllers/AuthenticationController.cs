using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSocketsChatStudy.DTOs;
using WebSocketsChatStudy.Services.Interfaces;

namespace WebSocketsChatStudy.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
    {
        var response = await _authenticationService.Login(loginDTO);
        return Ok(response);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        await _authenticationService.Register(registerUserDTO);
        return Created();
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<ActionResult<TokenDTO>> RefreshToken([FromBody] TokenDTO tokenDTO)
    {
        var response = await _authenticationService.RefreshToken(tokenDTO);
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    [Route("RevokeAccess/{email}")]
    public async Task<IActionResult> RevokeAccess(string email)
    {
        await _authenticationService.RekoveAccess(email);

        return NoContent();
    }
}

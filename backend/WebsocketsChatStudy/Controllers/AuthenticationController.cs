using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebSocketsChatStudy.DTOs;
using WebSocketsChatStudy.Exceptions;
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
        try
        {
            var response = await _authenticationService.Login(loginDTO);
            return Ok(response);
        }
        catch (InvalidCredentialsException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        try
        {
            await _authenticationService.Register(registerUserDTO);
            return Created();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<ActionResult<TokenDTO>> RefreshToken([FromBody] TokenDTO tokenDTO)
    {
        try
        {
            var response = await _authenticationService.RefreshToken(tokenDTO);
            return Ok(response);
        }
        catch (InvalidCredentialsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [Authorize]
    [HttpPost]
    [Route("RevokeAccess")]
    public async Task<IActionResult> RevokeAccess()
    {
        try
        {
            string? userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _authenticationService.RekoveAccess(userId!);
            return NoContent();

        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

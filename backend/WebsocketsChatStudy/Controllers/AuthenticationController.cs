using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

}

using Microsoft.AspNetCore.Mvc;
using WebSocketsChatStudy.DTOs;
using WebSocketsChatStudy.Services.Interfaces;

namespace WebSocketsChatStudy.Services;

public class AuthenticationService : IAuthenticationService
{
    public Task<IActionResult> Login(LoginDTO loginDTO)
    {
        throw new NotImplementedException();
    }
}

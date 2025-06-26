using Microsoft.AspNetCore.Mvc;
using WebSocketsChatStudy.DTOs;

namespace WebSocketsChatStudy.Services.Interfaces;

public interface IAuthenticationService
{
    Task<IActionResult> Login(LoginDTO loginDTO);
}

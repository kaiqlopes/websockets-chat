using Microsoft.AspNetCore.Mvc;
using WebSocketsChatStudy.DTOs;

namespace WebSocketsChatStudy.Services.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponseDTO> Login(LoginDTO loginDTO);
    Task Register(RegisterUserDTO registerUserDTO);
    Task<TokenDTO> RefreshToken(TokenDTO tokenDTO);
    Task RekoveAccess(string userId);
}

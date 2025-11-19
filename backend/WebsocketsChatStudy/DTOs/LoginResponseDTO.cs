using System.IdentityModel.Tokens.Jwt;

namespace WebSocketsChatStudy.DTOs;

public class LoginResponseDTO
{
    public required long UserId { get; set; }
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}

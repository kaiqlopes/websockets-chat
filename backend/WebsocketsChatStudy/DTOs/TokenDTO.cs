using System.ComponentModel.DataAnnotations;

namespace WebSocketsChatStudy.DTOs;

public class TokenDTO
{
    [Required(ErrorMessage = "Invalid access token")]
    public string? AccessToken { get; set; }

    [Required(ErrorMessage = "Invalid refresh token")]
    public string? RefreshToken { get; set; }
}

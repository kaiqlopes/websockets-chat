using WebSocketsChatStudy.DTOs;

namespace WebSocketsChatStudy.Services.Interfaces;

public interface IChatService
{
    public Task<UserRecipientDTO?> GetUserRecipientByEmailAsync(string email);
}

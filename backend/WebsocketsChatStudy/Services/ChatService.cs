using Microsoft.EntityFrameworkCore;
using WebSocketsChatStudy.Context;
using WebSocketsChatStudy.DTOs;
using WebSocketsChatStudy.Services.Interfaces;

namespace WebSocketsChatStudy.Services;

public class ChatService : IChatService
{
    private readonly ChatContext _chatContext;
    public ChatService(ChatContext chatContext) => _chatContext = chatContext;
    public async Task<UserRecipientDTO?> GetUserRecipientByEmailAsync(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.");

        var user = await _chatContext.Users
            .Where(x => x.Email == email)
            .Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.ImageUrl,
                x.Email
            })
            .FirstOrDefaultAsync();

        if(user == null)
            return null;

        var userDto = new UserRecipientDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ImageUrl = user.ImageUrl,
            Email = user.Email
        };

        return userDto;
    }
}

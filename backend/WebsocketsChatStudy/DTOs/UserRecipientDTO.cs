namespace WebSocketsChatStudy.DTOs;

public class UserRecipientDTO
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public string? Email { get; set; }
}

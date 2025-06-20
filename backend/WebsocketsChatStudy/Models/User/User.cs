using Microsoft.AspNetCore.Identity;

namespace WebSocketsChatStudy.Models.User;

public class User : IdentityUser<long>
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
}

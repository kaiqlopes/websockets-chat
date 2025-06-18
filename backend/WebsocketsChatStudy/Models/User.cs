using Microsoft.AspNetCore.Identity;

namespace WebSocketsChatStudy.Models;

public class User : IdentityUser
{

    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
}

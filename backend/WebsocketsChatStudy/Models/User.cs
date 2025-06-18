using Microsoft.AspNetCore.Identity;

namespace WebSocketsChatStudy.Models;

public class User : IdentityUser
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<User>? Contacts { get; }
}

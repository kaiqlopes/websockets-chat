namespace WebSocketsChatStudy.Entities;

public class User
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public IEnumerable<User>? Contacts { get; }
}

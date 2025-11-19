using System.ComponentModel.DataAnnotations;

namespace WebSocketsChatStudy.DTOs;

public class RegisterUserDTO
{
	[Required(ErrorMessage = "First name is required")]
	public required string FirstName { get; set; }
	public string? LastName { get; set; }

	[EmailAddress]
	[Required(ErrorMessage = "Email is required")]
	public required string Email { get; set; }

	[Required(ErrorMessage = "Password is required")]
	public string? Password { get; set; }
}

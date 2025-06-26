using System.ComponentModel.DataAnnotations;

namespace WebSocketsChatStudy.DTOs;

public class CreateUserDTO
{
	[Required(ErrorMessage = "First name is required")]
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? ImageUrl { get; set; }

	[EmailAddress]
	[Required(ErrorMessage = "Email is required")]
	public string? Email { get; set; }

	[Required(ErrorMessage = "Password is required")]
	public string? Password { get; set; }
}

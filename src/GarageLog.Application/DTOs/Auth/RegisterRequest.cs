using System.ComponentModel.DataAnnotations;

namespace GarageLog.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [MinLength(3, ErrorMessage = "First name must be at least 3 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MinLength(3, ErrorMessage = "Last name must be at least 3 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; } = string.Empty;
}

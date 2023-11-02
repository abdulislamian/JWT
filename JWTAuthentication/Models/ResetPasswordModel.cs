using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models;

public class ResetPasswordModel
{
    public string UserId { get; set; }
    public string Token { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}

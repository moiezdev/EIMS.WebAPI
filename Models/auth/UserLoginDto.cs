using System.ComponentModel.DataAnnotations;

namespace EIMS.WebAPI.Models
{
    public class UserLoginDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
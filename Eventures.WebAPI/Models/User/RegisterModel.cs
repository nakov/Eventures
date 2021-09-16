using System.ComponentModel.DataAnnotations;

namespace Eventures.WebAPI.Models.User
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; init; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid!")]
        public string Email { get; init; }

        [Required]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; init; }

        [Required]
        public string FirstName { get; init; }

        [Required]
        public string LastName { get; init; }
    }
}

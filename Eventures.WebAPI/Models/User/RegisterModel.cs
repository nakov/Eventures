using System.ComponentModel.DataAnnotations;

using Eventures.Data;

namespace Eventures.WebAPI.Models.User
{
    using static DataConstants;
    public class RegisterModel
    {
        [Required]
        [StringLength(MaxUserUsername)]
        public string Username { get; init; }

        [Required]
        [EmailAddress]
        [StringLength(MaxUserEmail)]
        public string Email { get; init; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; init; }

        [Required]
        [StringLength(MaxUserFirstName)]
        public string FirstName { get; init; }

        [Required]
        [StringLength(MaxUserFirstName)]
        public string LastName { get; init; }
    }
}

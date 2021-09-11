using System.ComponentModel.DataAnnotations;

namespace Eventures.WebAPI.Models.User
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
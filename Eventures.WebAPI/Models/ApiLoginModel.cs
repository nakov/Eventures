using System.ComponentModel.DataAnnotations;

namespace Eventures.WebAPI.Models
{
    public class ApiLoginModel
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}
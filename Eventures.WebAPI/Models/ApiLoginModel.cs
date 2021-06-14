using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ApiLoginModel
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}
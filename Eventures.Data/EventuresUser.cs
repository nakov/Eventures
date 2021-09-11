using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Eventures.Data
{
    public class EventuresUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}

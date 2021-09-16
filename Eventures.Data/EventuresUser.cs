using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

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

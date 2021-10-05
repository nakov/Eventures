using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Eventures.Data
{
    using static DataConstants;

    public class EventuresUser : IdentityUser
    {
        [Required]
        [MaxLength(MaxUserFirstName)]
        public string FirstName { get; init; }

        [Required]
        [MaxLength(MaxUserLastName)]
        public string LastName { get; init; }
    }
}

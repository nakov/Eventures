using Microsoft.AspNetCore.Identity;

namespace Eventures.Data
{
    public class EventuresUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

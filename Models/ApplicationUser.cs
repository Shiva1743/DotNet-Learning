using Microsoft.AspNetCore.Identity;

namespace CoreEmptyProject1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}

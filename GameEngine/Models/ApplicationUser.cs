using Microsoft.AspNetCore.Identity;

namespace GameEngine.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Address { get; set; }
    }
}

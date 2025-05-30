using Microsoft.AspNetCore.Identity;

namespace Agency.AuthAPI.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

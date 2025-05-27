using Microsoft.AspNetCore.Identity;

namespace Agency.AuthAPI.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

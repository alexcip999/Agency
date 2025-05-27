using Agency.AuthAPI.Domain.Entities;

namespace Agency.AuthAPI.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser application);
    }
}

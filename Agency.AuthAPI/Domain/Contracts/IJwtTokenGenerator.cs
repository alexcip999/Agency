using Agency.AuthAPI.Infrastructure.Entities;

namespace Agency.AuthAPI.Domain.Contracts
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser application, IEnumerable<string> roles);
    }
}

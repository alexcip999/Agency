using Agency.AuthAPI.Domain.Dto;

namespace Agency.AuthAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string emai, string roleName);
    }
}

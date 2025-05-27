using Agency.Web.Domain.Dto;
using Agency.Webb.Domain.Dto;

namespace Agency.Web.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegisterRequestDto registerRequestDto);
    }
}

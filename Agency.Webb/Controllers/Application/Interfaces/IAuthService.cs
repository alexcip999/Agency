using Agency.Web.Models.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Web.Controllers.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegisterRequestDto registerRequestDto);

        Task<ResponseDto?> GetAllUsersAsync();
        Task<ResponseDto?> DeleteUserAsync(Guid userId);
        Task<ResponseDto?> ChangeRoleUserAsync(Guid userId, string newRole);
        Task<ResponseDto?> EditUserAsync(UserDto userDto);
        Task<FileContentResult?> ExportCSVAsync();
    }
}

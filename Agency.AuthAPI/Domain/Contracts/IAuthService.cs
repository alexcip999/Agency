using Agency.AuthAPI.Domain.Dto;

namespace Agency.AuthAPI.Domain.Contracts
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string emai, string roleName);
        Task<bool> DeleteUser(Guid userId);
        Task<bool> ChangeRoleUser(Guid userId, string newRole);
        Task<bool> EditUser(Guid userId, string newName, string newEmail, string newPhone);
        Task<string> ExportCSV();
        Task<List<UserDto>> GetAllUsers();
    }
}

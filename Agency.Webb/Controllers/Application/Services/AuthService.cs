using Agency.Web.Controllers.Application.Interfaces;
using Agency.Web.Models.Domain.Dto;
using Agency.Web.Models.Domain.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Web.Controllers.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegisterRequestDto registerRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registerRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registerRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> GetAllUsersAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/GetAllUsers"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> DeleteUserAsync(Guid userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.AuthAPIBase + $"/api/auth/DeleteUser/{userId}"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> ChangeRoleUserAsync(Guid userId, string newRole)
        {
            var data = new { UserId = userId, NewRole = newRole };
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = data,
                Url = SD.AuthAPIBase + "/api/auth/ChangeRoleUser"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> EditUserAsync(UserDto userDto)
        {
            var data = new
            {
                UserId = Guid.Parse(userDto.Id),
                NewName = userDto.Name,
                NewEmail = userDto.Email,
                NewPhone = userDto.PhoneNumber
            };
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = data,
                Url = SD.AuthAPIBase + "/api/auth/EditUser"
            }, withBearer: false);
        }

        public async Task<FileContentResult?> ExportCSVAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(SD.AuthAPIBase + "/api/auth/ExportCSV");
            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                return new FileContentResult(bytes, "text/csv") { FileDownloadName = "users.csv" };
            }
            return null;
        }
    }
}

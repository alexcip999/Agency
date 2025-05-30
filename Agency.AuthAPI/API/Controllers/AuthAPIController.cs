using Agency.AuthAPI.Domain.Contracts;
using Agency.AuthAPI.Domain.Dto;
using Agency.Services.AuthAPI.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Agency.AuthAPI.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect.";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role?.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encourated";
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _authService.DeleteUser(userId);
            if (!result)
            {
                _response.IsSuccess = false;
                _response.Message = "User not found or could not be deleted.";
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.Message = "User deleted successfully.";
            return Ok(_response);
        }

        [HttpPost("ChangeRoleUser")]
        public async Task<IActionResult> ChangeRoleUser([FromBody] ChangeRoleUserDto model)
        {
            var result = await _authService.ChangeRoleUser(model.UserId, model.NewRole);
            if (!result)
            {
                _response.IsSuccess = false;
                _response.Message = "Role change failed.";
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Message = "Role changed successfully.";
            return Ok(_response);
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] EditUserDto model)
        {
            var result = await _authService.EditUser(model.UserId, model.NewName, model.NewEmail, model.NewPhone);
            if (!result)
            {
                _response.IsSuccess = false;
                _response.Message = "User update failed.";
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Message = "User updated successfully.";
            return Ok(_response);
        }

        [HttpGet("ExportCSV")]
        public async Task<IActionResult> ExportCSV()
        {
            var csv = await _authService.ExportCSV();
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", "users.csv");
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsers();
            _response.Result = users;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }

}

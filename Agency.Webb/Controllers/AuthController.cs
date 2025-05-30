using Agency.Web.Controllers.Application.Interfaces;
using Agency.Web.Models.Domain.Dto;
using Agency.Web.Models.Domain.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Agency.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();

            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                await SignInUser(loginResponseDto);

                _tokenProvider.SetToken(loginResponseDto.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(loginRequestDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleClient, Value = SD.RoleClient},
                new SelectListItem{Text = SD.RoleEmployee, Value = SD.RoleEmployee},
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleManager, Value = SD.RoleManager},
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            ResponseDto result = await _authService.RegisterAsync(registerRequestDto);
            ResponseDto assignRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registerRequestDto.Role))
                {
                    registerRequestDto.Role = SD.RoleClient;
                }

                assignRole = await _authService.AssignRoleAsync(registerRequestDto);

                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "User registered successfully";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleClient, Value = SD.RoleClient},
                new SelectListItem{Text = SD.RoleEmployee, Value = SD.RoleEmployee},
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleManager, Value = SD.RoleManager},
            };

            ViewBag.RoleList = roleList;

            return View(registerRequestDto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();

            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var response = await _authService.GetAllUsersAsync();
            var users = response != null && response.IsSuccess
                ? JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(response.Result))
                : new List<UserDto>();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _authService.DeleteUserAsync(Guid.Parse(id));
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoleUser(string id, string newRole)
        {
            await _authService.ChangeRoleUserAsync(Guid.Parse(id), newRole);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserDto user)
        {
            await _authService.EditUserAsync(user);
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> ExportCSV()
        {
            var fileResult = await _authService.ExportCSVAsync();
            if (fileResult != null)
            {
                return fileResult;
            }
            return NotFound();
        }
    }
}

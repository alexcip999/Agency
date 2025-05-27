using Agency.Web.Application.Interfaces;
using Agency.Web.Domain.Dto;
using Agency.Webb.Domain.Dto;
using Agency.Webb.Domain.Utility;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Agency.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            
            return View(loginRequestDto);
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
        public IActionResult Logout()
        {
            return View();
        }
    }
}

using Agency.AuthAPI.Domain.Contracts;
using Agency.AuthAPI.Domain.Dto;
using Agency.AuthAPI.Infrastructure.Entities;
using Agency.Services.AuthAPI.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Agency.AuthAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string emai, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == emai.ToLower());

            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            // if user was found and password is valid, generate token

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegisterRequestDto registerRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                Name = registerRequestDto.Name,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registerRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Id = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while registering the user.", ex);
            }

            return "Error Encourated";
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangeRoleUser(Guid userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            if (!await _roleManager.RoleExistsAsync(newRole))
                await _roleManager.CreateAsync(new IdentityRole(newRole));

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            return addResult.Succeeded;
        }

        public async Task<bool> EditUser(Guid userId, string newName, string newEmail, string newPhone)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            user.Name = newName;
            user.Email = newEmail;
            user.UserName = newEmail;
            user.PhoneNumber = newPhone;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<string> ExportCSV()
        {
            var users = await _userManager.Users.ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Id,Email,Name,PhoneNumber");

            foreach (var user in users)
            {
                sb.AppendLine($"{user.Id},{user.Email},{user.Name},{user.PhoneNumber}");
            }

            return sb.ToString();
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                PhoneNumber = u.PhoneNumber
            }).ToList();
        }
    }
}

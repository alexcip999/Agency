using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models.Domain.Dto
{
    public class RegisterRequestDto
    {
        [Required] public string Email { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Password { get; set; }
        public string? Role { get; set; } // Optional role for the user
    }
}

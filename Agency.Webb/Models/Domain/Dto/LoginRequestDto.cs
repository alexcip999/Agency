﻿using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models.Domain.Dto
{
    public class LoginRequestDto
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
    }
}

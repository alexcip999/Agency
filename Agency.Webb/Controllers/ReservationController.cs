using Agency.Web.Controllers.Application.Interfaces;
using Agency.Web.Models.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Agency.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IPropertyService _propertyService;

        public ReservationController(IReservationService reservationService, IPropertyService propertyService)
        {
            _reservationService = reservationService;
            _propertyService = propertyService;
        }

        [Authorize]
        public async Task<IActionResult> ReservationIndex()
        {
            return View(await LoadReservationBasedOnLoggedInUser());
        }

        private async Task<ReservationDto> LoadReservationBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _reservationService.GetReservationByUserIdAsync(Guid.Parse(userId));
            if (response != null && response.IsSuccess)
            {
                ReservationDto reservationDto = JsonConvert.DeserializeObject<ReservationDto>(Convert.ToString(response.Result));
                if (reservationDto?.ReservationDetails != null)
                {
                    foreach (var detail in reservationDto.ReservationDetails)
                    {
                        var propertyResponse = await _propertyService.GetPropertyByIdAsync(detail.PropertyId);
                        if (propertyResponse != null && propertyResponse.IsSuccess)
                        {
                            detail.Property = JsonConvert.DeserializeObject<PropertyDto>(Convert.ToString(propertyResponse.Result));
                        }
                    }
                }
                return reservationDto;
            }
            return new ReservationDto();
        }
    }
}

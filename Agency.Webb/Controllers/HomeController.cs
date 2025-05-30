using Agency.Web.Controllers.Application.Interfaces;
using Agency.Web.Models.Domain.Dto;
using Agency.Web.Models.Domain.Utility;
using Agency.Webb.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Agency.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IReservationService _reservationService;
        public HomeController(IPropertyService propertyService, IReservationService reservationService)
        {
            _propertyService = propertyService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            List<PropertyDto>? list = new();

            ResponseDto? response = await _propertyService.GetPropertiesAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<PropertyDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> PropertyDetails(Guid propertyId)
        {
            PropertyDto? property = new();

            ResponseDto? response = await _propertyService.GetPropertyByIdAsync(propertyId);

            if (response != null && response.IsSuccess)
            {
                property = JsonConvert.DeserializeObject<PropertyDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(property);
        }

        [HttpPost]
        [ActionName("PropertyDetails")]
        public async Task<IActionResult> PropertyDetails(PropertyDto propertyDto)
        {
            ReservationDto reservationDto = new ReservationDto()
            {
                ReservationHeader = new ReservationHeaderDto()
                {
                    UserId = Guid.TryParse(User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value, out var userId) ? userId : null
                }
            };

            ReservationDetailsDto reservationDetails = new ReservationDetailsDto()
            {
                PropertyId = propertyDto.PropertyId
            };

            List<ReservationDetailsDto> reservationDetailsDtos = new() { reservationDetails };

            reservationDto.ReservationDetails = reservationDetailsDtos;

            ResponseDto? response = await _reservationService.UpsertReservationAsync(reservationDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Reservation created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(propertyDto);
        }

        [Authorize(Roles = SD.RoleAdmin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Agency.Web.Controllers.Application.Interfaces;
using Agency.Web.Models.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Agency.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        public async Task<IActionResult> PropertyIndex()
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

        public async Task<IActionResult> PropertyCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PropertyCreate(PropertyDto model)
        {

            if (ModelState.IsValid)
            {
                ResponseDto? response = await _propertyService.CreatePropertyAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Property created successfully";
                    return RedirectToAction(nameof(PropertyIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }


            return View(model);
        }


        public async Task<IActionResult> PropertyDelete(Guid propertyId)
        {

            ResponseDto? response = await _propertyService.GetPropertyByIdAsync(propertyId);

            if (response != null && response.IsSuccess)
            {
                PropertyDto? model = JsonConvert.DeserializeObject<PropertyDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PropertyDelete(PropertyDto propertyDto)
        {

            ResponseDto? response = await _propertyService.DeletePropertyAsync(propertyDto.PropertyId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Property deleted successfully";
                return RedirectToAction(nameof(PropertyIndex));
            }
            else
            {
                TempData["Error"] = response?.Message;
            }


            return View(propertyDto);
        }

        public async Task<IActionResult> PropertyEdit(Guid propertyId)
        {

            ResponseDto? response = await _propertyService.GetPropertyByIdAsync(propertyId);

            if (response != null && response.IsSuccess)
            {
                PropertyDto? model = JsonConvert.DeserializeObject<PropertyDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PropertyEdit(PropertyDto propertyDto)
        {

            ResponseDto? response = await _propertyService.UpdatePropertyAsync(propertyDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Property updated successfully";
                return RedirectToAction(nameof(PropertyIndex));
            }
            else
            {
                TempData["Error"] = response?.Message;
            }


            return View(propertyDto);
        }
    }
}

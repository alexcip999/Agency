using Agency.Webb.Application.Interfaces;
using Agency.Webb.Domain.Dto;
using Agency.Webb.Domain.Utility;

namespace Agency.Webb.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IBaseService _baseService;

        public PropertyService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreatePropertyAsync(PropertyDto propertyDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = propertyDto,
                Url = SD.PropertyAPIBase + "/api/property"
            });
        }

        public async Task<ResponseDto?> DeletePropertyAsync(Guid id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.PropertyAPIBase + $"/api/property/{id}"
            });
        }

        public async Task<ResponseDto?> GetPropertiesAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.PropertyAPIBase + "/api/property"
            });
        }

        public async Task<ResponseDto?> GetPropertyByIdAsync(Guid id)
        {
            return await _baseService.SendAsync(new RequestDto 
            { 
                ApiType = SD.ApiType.GET, 
                Url = SD.PropertyAPIBase + $"/api/property/{id}" 
            });
        }

        public async Task<ResponseDto?> UpdatePropertyAsync(PropertyDto propertyDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = propertyDto,
                Url = SD.PropertyAPIBase + "/api/property"
            });
        }
    }
}

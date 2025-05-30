using Agency.Services.ReservationAPI.Domain.Contracts;
using Agency.Services.ReservationAPI.Domain.Dto;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Agency.Services.ReservationAPI.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PropertyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<PropertyDto>> GetProperties()
        {
            var client = _httpClientFactory.CreateClient("PropertyServiceClient");
            var response = await client.GetAsync($"api/property");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<PropertyDto>>(Convert.ToString(resp.Result));
            }
            return new List<PropertyDto>();
        }

        public async Task<PropertyDto?> GetPropertyById(Guid propertyId)
        {
            var client = _httpClientFactory.CreateClient("PropertyServiceClient");
            var response = await client.GetAsync($"api/property/{propertyId}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<PropertyDto>(Convert.ToString(resp.Result));
            }
            return null;
        }
    }
}

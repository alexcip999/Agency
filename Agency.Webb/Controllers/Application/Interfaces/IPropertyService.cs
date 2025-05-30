using Agency.Web.Models.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Web.Controllers.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<ResponseDto?> GetPropertiesAsync();
        Task<ResponseDto?> GetPropertyByIdAsync(Guid id);
        Task<ResponseDto?> CreatePropertyAsync(PropertyDto propertyDto);
        Task<ResponseDto?> UpdatePropertyAsync(PropertyDto propertyDto);
        Task<ResponseDto?> DeletePropertyAsync(Guid id);
    }
}

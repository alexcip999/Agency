using Agency.Webb.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Webb.Application.Interfaces
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

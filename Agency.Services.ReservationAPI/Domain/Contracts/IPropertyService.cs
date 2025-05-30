using Agency.Services.ReservationAPI.Domain.Dto;

namespace Agency.Services.ReservationAPI.Domain.Contracts
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyDto>> GetProperties();
        Task<PropertyDto?> GetPropertyById(Guid propertyId);
    }
}

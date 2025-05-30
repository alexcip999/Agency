using Agency.Web.Models.Domain.Dto;

namespace Agency.Web.Controllers.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ResponseDto?> GetReservationByUserIdAsync(Guid userId);
        Task<ResponseDto?> UpsertReservationAsync(ReservationDto reservationDto);
        Task<ResponseDto?> RemoveReservationAsync(Guid reservationDetailsId);
    }
}

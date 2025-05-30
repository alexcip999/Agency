using Agency.Web.Controllers.Application.Interfaces;
using Agency.Web.Models.Domain.Dto;
using Agency.Web.Models.Domain.Utility;

namespace Agency.Web.Controllers.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IBaseService _baseService;

        public ReservationService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> GetReservationByUserIdAsync(Guid userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ReservationAPIBase + $"/api/reservation/GetReservation/{userId}"
            });
        }

        public async Task<ResponseDto?> RemoveReservationAsync(Guid reservationDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = reservationDetailsId,
                Url = SD.ReservationAPIBase + "/api/reservation/RemoveReservation"
            });
        }

       

        public async Task<ResponseDto?> UpsertReservationAsync(ReservationDto reservationDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = reservationDto,
                Url = SD.ReservationAPIBase + "/api/reservation/ReservationUpsert"
            });
        }
    }
}

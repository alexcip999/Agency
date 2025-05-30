using Agency.Services.ReservationAPI.Domain.Contracts;
using Agency.Services.ReservationAPI.Domain.Dto;
using Agency.Services.ReservationAPI.Infrastructure.Contexts;
using Agency.Services.ReservationAPI.Infrastructure.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Services.ReservationAPI.API.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IPropertyService _propertyService;

        public ReservationAPIController(AppDbContext db, IMapper mapper, IPropertyService propertyService)
        {
            _db = db;
            _mapper = mapper;
            this._response = new ResponseDto();
            _propertyService = propertyService;
        }

        [HttpGet("GetReservation/{userId}")]
        public async Task<ResponseDto> GetCart(Guid userId)
        {
            try
            {
                ReservationDto reservation = new()
                {
                    ReservationHeader = _mapper.Map<ReservationHeaderDto>(_db.ReservationHeaders.First(u => u.UserId == userId))
                };
                reservation.ReservationDetails = _mapper.Map<IEnumerable<ReservationDetailsDto>>(_db.ReservationDetails
                    .Where(u => u.ReservationHeaderId == reservation.ReservationHeader.ReservationId));

                _response.Result = reservation;
            } 
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }

            return _response;
        }

        [HttpPost("ReservationUpsert")]
        public async Task<ResponseDto> ReservationUpsert(ReservationDto reservationDto)
        {
            try
            {
                var reservationHeaderFromDb = await _db.ReservationHeaders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == reservationDto.ReservationHeader.UserId);

                if (reservationHeaderFromDb == null)
                {
                    // create header and details
                    ReservationHeader reservationHeader = _mapper.Map<ReservationHeader>(reservationDto.ReservationHeader);
                    reservationHeader.ReservationDate = DateTime.UtcNow; // Set the date here
                    _db.ReservationHeaders.Add(reservationHeader);
                    await _db.SaveChangesAsync();
                    reservationDto.ReservationDetails.First().ReservationHeaderId = reservationHeader.ReservationId;
                    reservationDto.ReservationDetails.First().Property = await _propertyService.GetPropertyById(reservationDto.ReservationDetails.First().PropertyId);
                    _db.ReservationDetails.Add(_mapper.Map<ReservationDetails>(reservationDto.ReservationDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // update header and details
                    ReservationHeader updatedHeader = _mapper.Map<ReservationHeader>(reservationDto.ReservationHeader);
                    updatedHeader.ReservationId = reservationHeaderFromDb.ReservationId;
                    updatedHeader.ReservationDate = reservationHeaderFromDb.ReservationDate != default
                        ? reservationHeaderFromDb.ReservationDate
                        : DateTime.UtcNow; // Keep original date or set if missing
                    _db.ReservationHeaders.Update(updatedHeader);

                    var reservationDetailsFromDb = await _db.ReservationDetails
                        .AsNoTracking()
                        .FirstOrDefaultAsync(
                            u => u.PropertyId == reservationDto.ReservationDetails.First().PropertyId &&
                            u.ReservationHeaderId == reservationHeaderFromDb.ReservationId);

                    if (reservationDetailsFromDb == null)
                    {
                        reservationDto.ReservationDetails.First().ReservationHeaderId = reservationHeaderFromDb.ReservationId;
                        _db.ReservationDetails.Add(_mapper.Map<ReservationDetails>(reservationDto.ReservationDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        reservationDto.ReservationDetails.First().ReservationDetailsId = reservationDetailsFromDb.ReservationDetailsId;
                        reservationDto.ReservationDetails.First().ReservationHeaderId = reservationHeaderFromDb.ReservationId;
                        _db.ReservationDetails.Update(_mapper.Map<ReservationDetails>(reservationDto.ReservationDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = reservationDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }

            return _response;
        }

        [HttpPost("RemoveReservation")]
        public async Task<ResponseDto> RemoveReservation([FromBody]Guid reservationDetailsId)
        {
            try
            {
                ReservationDetails reservationDetails = _db.ReservationDetails
                    .First(u => u.ReservationDetailsId == reservationDetailsId);

                int totalCountReservationItem = _db.ReservationDetails.Where(u => u.ReservationHeaderId == reservationDetails.ReservationHeaderId).Count();
                _db.ReservationDetails.Remove(reservationDetails);
                if (totalCountReservationItem == 1)
                {
                    var reservationHeaderToRemove = await _db.ReservationHeaders
                        .FirstOrDefaultAsync(u => u.ReservationId == reservationDetails.ReservationHeaderId);
                    _db.ReservationHeaders.Remove(reservationHeaderToRemove);
                }

                await _db.SaveChangesAsync();
                
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }

            return _response;
        }
    }
}

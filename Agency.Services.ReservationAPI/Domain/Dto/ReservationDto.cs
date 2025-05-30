namespace Agency.Services.ReservationAPI.Domain.Dto
{
    public class ReservationDto
    {
        public ReservationHeaderDto ReservationHeader { get; set; }
        public IEnumerable<ReservationDetailsDto>? ReservationDetails { get; set; }
    }
}

namespace Agency.Services.ReservationAPI.Domain.Dto
{
    public class ReservationHeaderDto
    {
        public Guid ReservationId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}

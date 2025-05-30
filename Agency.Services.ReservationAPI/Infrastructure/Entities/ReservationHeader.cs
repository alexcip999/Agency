using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agency.Services.ReservationAPI.Infrastructure.Entities
{
    public class ReservationHeader
    {
        [Key] public Guid ReservationId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}

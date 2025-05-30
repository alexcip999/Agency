using Agency.Services.ReservationAPI.Domain.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agency.Services.ReservationAPI.Infrastructure.Entities
{
    public class ReservationDetails
    {
        [Key] public Guid ReservationDetailsId { get; set; }
        public Guid ReservationHeaderId { get; set; }
        [ForeignKey("ReservationHeaderId")]
        public ReservationHeader ReservationHeader { get; set; }
        public Guid PropertyId { get; set; }
        [NotMapped]
        public PropertyDto Property { get; set; }
    }
}

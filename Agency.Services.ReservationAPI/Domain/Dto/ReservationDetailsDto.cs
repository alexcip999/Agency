﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agency.Services.ReservationAPI.Domain.Dto
{
    public class ReservationDetailsDto
    {
        public Guid ReservationDetailsId { get; set; }
        public Guid ReservationHeaderId { get; set; }
        public ReservationHeaderDto? ReservationHeader { get; set; }
        public Guid PropertyId { get; set; }
        public PropertyDto? Property { get; set; }
    }
}

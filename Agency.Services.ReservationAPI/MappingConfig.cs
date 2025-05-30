using Agency.Services.ReservationAPI.Domain.Dto;
using Agency.Services.ReservationAPI.Infrastructure.Entities;
using AutoMapper;

namespace Agency.Services.ReservationAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ReservationHeader, ReservationHeaderDto>().ReverseMap();
                config.CreateMap<ReservationDetails, ReservationDetailsDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}

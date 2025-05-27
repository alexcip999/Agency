using Agency.Services.PropertyAPI.Domain.Dto;
using Agency.Services.PropertyAPI.Domain.Entities;
using AutoMapper;

namespace Agency.Services.PropertyAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Property, PropertyDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}

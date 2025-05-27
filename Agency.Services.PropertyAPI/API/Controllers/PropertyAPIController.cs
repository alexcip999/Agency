using Agency.Services.PropertyAPI.Domain.Dto;
using Agency.Services.PropertyAPI.Domain.Entities;
using Agency.Services.PropertyAPI.Infrastructure.Contexts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Services.PropertyAPI.API.Controllers
{
    [Route("api/property")]
    [ApiController]
    public class PropertyAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;

        public PropertyAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetProperties()
        {
            try
            {
                IEnumerable<Property> properties = _db.Properties.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<PropertyDto>>(properties);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public ResponseDto GetPropertyById(Guid id)
        {
            try
            {
                Property property = _db.Properties.FirstOrDefault(p => p.PropertyId == id);
                _responseDto.Result = _mapper.Map<PropertyDto>(property);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPost]
        public ResponseDto CreateProperty([FromBody] PropertyDto propertyDto)
        {
            try
            {
                if (propertyDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Property data is null";
                    return _responseDto;
                }
                Property property = _mapper.Map<Property>(propertyDto);
                property.PropertyId = Guid.NewGuid(); // Generate a new ID
                _db.Properties.Add(property);
                _db.SaveChanges();
                _responseDto.Result = _mapper.Map<PropertyDto>(property);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        public ResponseDto UpdateProperty([FromBody] PropertyDto propertyDto)
        {
            try
            {
                if (propertyDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Property data is null";
                    return _responseDto;
                }
                Property property = _mapper.Map<Property>(propertyDto);
                _db.Properties.Update(property);
                _db.SaveChanges();
                _responseDto.Result = _mapper.Map<PropertyDto>(property);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        public ResponseDto DeleteProperty(Guid id)
        {
            try
            {
                Property property = _db.Properties.First(u => u.PropertyId == id);
                _db.Properties.Remove(property);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}

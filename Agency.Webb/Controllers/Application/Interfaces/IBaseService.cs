using Agency.Web.Models.Domain.Dto;

namespace Agency.Web.Controllers.Application.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}

using Agency.Web.Domain.Dto;

namespace Agency.Web.Application.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}

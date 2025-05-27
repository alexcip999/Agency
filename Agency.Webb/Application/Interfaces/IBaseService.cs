using Agency.Webb.Domain.Dto;

namespace Agency.Webb.Application.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}

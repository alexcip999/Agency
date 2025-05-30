namespace Agency.Web.Controllers.Application.Interfaces
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}

namespace Agency.Web.Models.Domain.Utility
{
    public class SD
    {
        public static string PropertyAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ReservationAPIBase { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleClient = "CLIENT";
        public const string RoleEmployee = "EMPLOYEE";
        public const string RoleManager = "MANAGER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}

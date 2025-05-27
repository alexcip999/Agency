namespace Agency.Web.Domain.Utility
{
    public class SD
    {
        public static string PropertyAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}

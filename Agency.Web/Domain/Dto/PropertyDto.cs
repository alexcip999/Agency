namespace Agency.Web.Domain.Dto
{
    public class PropertyDto
    {
        public Guid PropertyId { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public string Type { get; set; } // e.g., Apartment, House, etc.
        public int NumberOfRooms { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // e.g., Available, Sold, etc.
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
    }
}

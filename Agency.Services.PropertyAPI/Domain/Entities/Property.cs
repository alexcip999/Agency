using System.ComponentModel.DataAnnotations;

namespace Agency.Services.PropertyAPI.Domain.Entities
{
    public class Property
    {
        [Key] public Guid PropertyId { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string Location { get; set; }
        [Required] public double Price { get; set; }
        [Required] public string Type { get; set; } // e.g., Apartment, House, etc.
        [Required] public int NumberOfRooms { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Status { get; set; } // e.g., Available, Sold, etc.
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }

    }
}

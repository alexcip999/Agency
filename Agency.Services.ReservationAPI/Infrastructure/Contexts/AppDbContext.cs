using Agency.Services.ReservationAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agency.Services.ReservationAPI.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<ReservationHeader> ReservationHeaders{ get; set; }
        public DbSet<ReservationDetails> ReservationDetails { get; set; }
    }
}

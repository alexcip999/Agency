using Agency.Services.PropertyAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agency.Services.PropertyAPI.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Property> Properties { get; set; }
    }
}

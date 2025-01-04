using Microsoft.EntityFrameworkCore;
using Models;
using Models.SubClasses;

namespace DAL
{
    public class LogistContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<Document> Documents { get; set; }

        public LogistContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

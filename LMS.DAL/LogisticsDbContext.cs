// LMS.DAL/LogisticsDbContext.cs
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LMS.DAL.Models;

namespace LMS.DAL
{
    public class LogisticsDbContext : DbContext
    {
        public LogisticsDbContext() : base("name=LogisticsDbContext") { }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            mb.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            mb.Entity<UserAccount>()
              .HasIndex(u => u.Username).IsUnique();

            mb.Entity<Order>()
              .HasOptional(o => o.Shipment)
              .WithOptionalPrincipal()
              .Map(m => m.MapKey("ShipmentId")); // đảm bảo FK 1-1 mềm

            mb.Entity<RouteStop>()
              .HasRequired(rs => rs.Shipment)
              .WithMany(s => s.RouteStops)
              .HasForeignKey(rs => rs.ShipmentId)
              .WillCascadeOnDelete(true);

            base.OnModelCreating(mb);
        }
    }
}

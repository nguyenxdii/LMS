using LMS.DAL.Models;
using LogisticsApp.DAL.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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
        public DbSet<RouteTemplate> RouteTemplates { get; set; }
        public DbSet<RouteTemplateStop> RouteTemplateStops { get; set; }
        public DbSet<ShipmentDriverLog> ShipmentDriverLogs { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            mb.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            mb.Entity<UserAccount>()
              .HasIndex(u => u.Username).IsUnique();

            mb.Entity<Order>()
              .HasOptional(o => o.Shipment)
              .WithOptionalPrincipal()
              .Map(m => m.MapKey("ShipmentId"));

            mb.Entity<RouteStop>()
              .HasRequired(rs => rs.Shipment)
              .WithMany(s => s.RouteStops)
              .HasForeignKey(rs => rs.ShipmentId)
              .WillCascadeOnDelete(true);

            mb.Entity<RouteTemplate>()
                .HasMany(rt => rt.Stops)
                .WithRequired(rs => rs.Template)
                .HasForeignKey(rs => rs.TemplateId)
                .WillCascadeOnDelete(false);

            mb.Entity<RouteTemplateStop>()
                .HasOptional(rs => rs.Warehouse)
                .WithMany()
                .HasForeignKey(rs => rs.WarehouseId);

            mb.Entity<RouteTemplate>()
               .HasRequired(rt => rt.FromWarehouse)
               .WithMany()
               .HasForeignKey(rt => rt.FromWarehouseId)
               .WillCascadeOnDelete(false);

            mb.Entity<RouteTemplate>()
               .HasRequired(rt => rt.ToWarehouse)
               .WithMany()
               .HasForeignKey(rt => rt.ToWarehouseId)
               .WillCascadeOnDelete(false);

            mb.Entity<ShipmentDriverLog>()
                .HasRequired(log => log.Shipment)
                .WithMany()
                .HasForeignKey(log => log.ShipmentId)
                .WillCascadeOnDelete(false);

            mb.Entity<ShipmentDriverLog>()
                .HasOptional(log => log.OldDriver)
                .WithMany()
                .HasForeignKey(log => log.OldDriverId)
                .WillCascadeOnDelete(false);

            mb.Entity<ShipmentDriverLog>()
                .HasRequired(log => log.NewDriver)
                .WithMany()
                .HasForeignKey(log => log.NewDriverId)
                .WillCascadeOnDelete(false);


            mb.Entity<Vehicle>()
                 .HasOptional(vehicle => vehicle.Driver)
                 .WithOptionalDependent(driver => driver.Vehicle)
                 .Map(m => m.MapKey("Driver_Id"));
            base.OnModelCreating(mb);
        }
    }
}
// LMS.DAL/LogisticsDbContext.cs
using LMS.DAL.Models;
using LogisticsApp.DAL.Models; // Đảm bảo namespace này đúng nếu RouteTemplate/Stop ở đây
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LMS.DAL
{
    public class LogisticsDbContext : DbContext
    {
        // Constructor, trỏ đến tên chuỗi kết nối trong App.config/Web.config
        public LogisticsDbContext() : base("name=LogisticsDbContext") { }

        // --- DbSet cho tất cả các bảng ---
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
        public DbSet<Vehicle> Vehicles { get; set; } // << THÊM DbSet CHO VEHICLE

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            // --- Cấu hình chung ---
            // Tắt xóa dữ liệu liên quan tự động khi xóa bản ghi cha (thường nên làm thủ công)
            mb.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            mb.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // --- Cấu hình riêng cho từng Entity ---

            // UserAccount: Đảm bảo Username là duy nhất
            mb.Entity<UserAccount>()
              .HasIndex(u => u.Username).IsUnique();

            // Order-Shipment: Mối quan hệ 1-1 mềm (ShipmentId trong Order là nullable)
            mb.Entity<Order>()
              .HasOptional(o => o.Shipment)       // Order có thể có hoặc không có Shipment
              .WithOptionalPrincipal()            // Shipment không bắt buộc phải có Order (mặc dù logic nghiệp vụ yêu cầu)
              .Map(m => m.MapKey("ShipmentId")); // Ánh xạ khóa ngoại ShipmentId trong bảng Order

            // Shipment-RouteStop: Mối quan hệ 1-Nhiều (Một Shipment có nhiều RouteStop)
            mb.Entity<RouteStop>()
              .HasRequired(rs => rs.Shipment)     // RouteStop bắt buộc phải thuộc về Shipment
              .WithMany(s => s.RouteStops)        // Shipment có collection RouteStops
              .HasForeignKey(rs => rs.ShipmentId) // Khóa ngoại là ShipmentId trong RouteStop
              .WillCascadeOnDelete(true);         // TỰ ĐỘNG XÓA RouteStop khi Shipment bị xóa (Cân nhắc kỹ!)

            // RouteTemplate-RouteTemplateStop: Mối quan hệ 1-Nhiều
            mb.Entity<RouteTemplate>()
                .HasMany(rt => rt.Stops)            // Một RouteTemplate có nhiều Stops
                .WithRequired(rs => rs.Template)    // Mỗi Stop yêu cầu phải có một Template
                .HasForeignKey(rs => rs.TemplateId) // Khóa ngoại là TemplateId trong RouteTemplateStop
                .WillCascadeOnDelete(false);        // KHÔNG xóa Stops khi xóa Template

            // RouteTemplateStop-Warehouse: Mối quan hệ Nhiều-1 (Một Stop thuộc về 1 Warehouse)
            mb.Entity<RouteTemplateStop>()
                .HasOptional(rs => rs.Warehouse)    // Một Stop có thể liên kết đến Warehouse (dùng HasOptional nếu WarehouseId là nullable)
                .WithMany()                         // Một Warehouse có thể xuất hiện trong nhiều Stop (không cần nav prop ngược lại trong Warehouse)
                .HasForeignKey(rs => rs.WarehouseId); // Khóa ngoại là WarehouseId trong RouteTemplateStop

            // RouteTemplate - Warehouse (From/To): Mối quan hệ Nhiều-1
            mb.Entity<RouteTemplate>()
               .HasRequired(rt => rt.FromWarehouse) // Bắt buộc có kho đi
               .WithMany()                          // Một Warehouse có thể là kho đi của nhiều Template
               .HasForeignKey(rt => rt.FromWarehouseId) // Khóa ngoại
               .WillCascadeOnDelete(false);          // Không xóa Kho khi xóa Template

            mb.Entity<RouteTemplate>()
               .HasRequired(rt => rt.ToWarehouse)   // Bắt buộc có kho đến
               .WithMany()                          // Một Warehouse có thể là kho đến của nhiều Template
               .HasForeignKey(rt => rt.ToWarehouseId)   // Khóa ngoại
               .WillCascadeOnDelete(false);          // Không xóa Kho khi xóa Template

            // ShipmentDriverLog - Shipment/Driver: Mối quan hệ Nhiều-1
            mb.Entity<ShipmentDriverLog>()
                .HasRequired(log => log.Shipment)    // Log phải thuộc về Shipment
                .WithMany()                          // Shipment có thể có nhiều Log (không cần nav prop trong Shipment)
                .HasForeignKey(log => log.ShipmentId)
                .WillCascadeOnDelete(false);         // Không xóa Log khi xóa Shipment

            mb.Entity<ShipmentDriverLog>()
                .HasOptional(log => log.OldDriver)   // Có thể không có tài xế cũ
                .WithMany()                          // Driver có thể là OldDriver nhiều lần
                .HasForeignKey(log => log.OldDriverId)
                .WillCascadeOnDelete(false);         // Không xóa Log khi xóa Driver

            mb.Entity<ShipmentDriverLog>()
                .HasRequired(log => log.NewDriver)   // Bắt buộc có tài xế mới
                .WithMany()                          // Driver có thể là NewDriver nhiều lần
                .HasForeignKey(log => log.NewDriverId)
                .WillCascadeOnDelete(false);         // Không xóa Log khi xóa Driver

            // *** CẤU HÌNH MỐI QUAN HỆ 1-1 GIỮA DRIVER VÀ VEHICLE ***
            mb.Entity<Driver>()
                .HasOptional(d => d.Vehicle)         // Driver có thể có hoặc không có Vehicle
                .WithOptionalPrincipal(v => v.Driver); // Vehicle cũng có thể có hoặc không có Driver, và liên kết ngược lại qua thuộc tính 'Driver'

            // --- Gọi hàm base cuối cùng ---
            base.OnModelCreating(mb);
        }
    }
}
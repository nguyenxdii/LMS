// LMS.DAL/LogisticsDbContext.cs
using LMS.DAL.Models;
using LogisticsApp.DAL.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LMS.DAL
{
    public class LogisticsDbContext : DbContext
    {
        // ... (Constructor và các DbSet giữ nguyên) ...
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
              .WillCascadeOnDelete(true); // Xem xét lại việc cascade delete ở đây nếu cần

            // === SỬA ĐỔI/THÊM PHẦN CẤU HÌNH RouteTemplate VÀ RouteTemplateStop ===

            // Bắt đầu từ RouteTemplate: Một Template có nhiều Stop
            mb.Entity<RouteTemplate>()
                .HasMany(rt => rt.Stops)            // Chỉ đến collection 'Stops'
                .WithRequired(rs => rs.Template)    // Mỗi Stop trong collection đó yêu cầu có 'Template'
                .HasForeignKey(rs => rs.TemplateId) // Khóa ngoại liên kết là 'TemplateId' trong RouteTemplateStop
                .WillCascadeOnDelete(false);        // KHÔNG xóa Stops khi xóa Template (quan trọng!)

            // (Bạn có thể bỏ đoạn cấu hình bắt đầu từ RouteTemplateStop cũ đi nếu muốn,
            // vì cấu hình ở trên đã định nghĩa đủ cả 2 chiều)
            /*
            mb.Entity<RouteTemplateStop>()
              .HasRequired(rts => rts.Template)
              .WithMany() // Bỏ đi vì đã có .HasMany ở trên
              .HasForeignKey(rts => rts.TemplateId);
            */

            // === CÁC CẤU HÌNH KHÁC GIỮ NGUYÊN HOẶC THÊM NẾU CẦN ===
            // Cấu hình mối quan hệ giữa RouteTemplateStop và Warehouse (NÊN CÓ)
            mb.Entity<RouteTemplateStop>()
                .HasOptional(rs => rs.Warehouse) // Một Stop có thể liên kết đến Warehouse (dùng HasOptional nếu WarehouseId là nullable, HasRequired nếu không nullable)
                .WithMany() // Một Warehouse có thể xuất hiện trong nhiều Stop
                .HasForeignKey(rs => rs.WarehouseId);

            // Cấu hình FromWarehouse/ToWarehouse cho RouteTemplate (NÊN CÓ)
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


            base.OnModelCreating(mb); // Gọi base ở cuối
        }
    }
}
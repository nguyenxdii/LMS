namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Fix : DbMigration
    {
        public override void Up()
        {
            // *** BƯỚC 1: XÓA KHÓA NGOẠI VÀ TẤT CẢ INDEX CŨ TRƯỚC ***
            DropForeignKey("dbo.RouteTemplateStops", "TemplateId", "dbo.RouteTemplates");
            DropIndex("dbo.RouteTemplateStops", new[] { "TemplateId" });
            DropIndex("dbo.RouteTemplateStops", new[] { "RouteTemplate_Id" });
            DropIndex("dbo.RouteTemplateStops", "IX_Template_Seq");

            // *** BƯỚC 2: XÓA CỘT CŨ VÀ ĐỔI TÊN CỘT MỚI ***
            DropColumn("dbo.RouteTemplateStops", "TemplateId");
            RenameColumn(table: "dbo.RouteTemplateStops", name: "RouteTemplate_Id", newName: "TemplateId");

            // *** BƯỚC 2.5 (MỚI): XÓA CÁC DÒNG CÓ TemplateId (MỚI) BỊ NULL ***
            Sql("DELETE FROM dbo.RouteTemplateStops WHERE TemplateId IS NULL");

            // *** BƯỚC 3: ĐẢM BẢO CỘT MỚI NOT NULL VÀ TẠO LẠI INDEX, FK ***
            AlterColumn("dbo.RouteTemplateStops", "TemplateId", c => c.Int(nullable: false)); // Bây giờ sẽ thành công
            CreateIndex("dbo.RouteTemplateStops", "TemplateId");
            AddForeignKey("dbo.RouteTemplateStops", "TemplateId", "dbo.RouteTemplates", "Id", cascadeDelete: false);
        }

        // Sửa lại Down() tương ứng (phức tạp hơn, có thể bỏ qua nếu không cần rollback)
        public override void Down()
        {
            // ... (Viết lại logic Down cho phù hợp nếu cần thiết)
            // Tạm thời có thể để trống hoặc chỉ làm các bước cơ bản nếu không rollback
            DropForeignKey("dbo.RouteTemplateStops", "TemplateId", "dbo.RouteTemplates");
            DropIndex("dbo.RouteTemplateStops", new[] { "TemplateId" });
            // AlterColumn("dbo.RouteTemplateStops", "TemplateId", c => c.Int()); // Có thể không cần nếu không rollback
            RenameColumn(table: "dbo.RouteTemplateStops", name: "TemplateId", newName: "RouteTemplate_Id");
            AddColumn("dbo.RouteTemplateStops", "TemplateId", c => c.Int(nullable: false)); // Thêm lại cột cũ
            CreateIndex("dbo.RouteTemplateStops", "RouteTemplate_Id");
            CreateIndex("dbo.RouteTemplateStops", "TemplateId");
            CreateIndex("dbo.RouteTemplateStops", "IX_Template_Seq"); // Tạo lại index đã xóa
            AddForeignKey("dbo.RouteTemplateStops", "TemplateId", "dbo.RouteTemplates", "Id", cascadeDelete: false); // Tạo lại FK cũ
        }
    }
}
//namespace LMS.DAL.Migrations
//{
//    using System;
//    using System.Data.Entity.Migrations;

//    public partial class AddRouteTemplateStop : DbMigration
//    {
//        public override void Up()
//        {
//            CreateTable(
//                "dbo.RouteTemplateStops",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        TemplateId = c.Int(nullable: false),
//                        Seq = c.Int(nullable: false),
//                        WarehouseId = c.Int(),
//                        StopName = c.String(maxLength: 200),
//                        PlannedOffsetHours = c.Double(),
//                        Note = c.String(maxLength: 200),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.RouteTemplates", t => t.TemplateId)
//                .Index(t => t.TemplateId);

//        }

//        public override void Down()
//        {
//            DropForeignKey("dbo.RouteTemplateStops", "TemplateId", "dbo.RouteTemplates");
//            DropIndex("dbo.RouteTemplateStops", new[] { "TemplateId" });
//            DropTable("dbo.RouteTemplateStops");
//        }
//    }
//}

using System.Data.Entity.Migrations;

namespace LMS.DAL.Migrations
{
    public partial class AddRouteTemplateStop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RouteTemplateStops",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TemplateId = c.Int(nullable: false),
                    Seq = c.Int(nullable: false),
                    WarehouseId = c.Int(),
                    StopName = c.String(maxLength: 200),
                    PlannedOffsetHours = c.Double(),
                    Note = c.String(maxLength: 200),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RouteTemplates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => new { t.TemplateId, t.Seq }, unique: true, name: "IX_Template_Seq");

            // (Optional) Nếu bạn muốn thêm index WarehouseId:
            CreateIndex("dbo.RouteTemplateStops", "WarehouseId");
        }

        public override void Down()
        {
            DropIndex("dbo.RouteTemplateStops", "IX_Template_Seq");
            DropIndex("dbo.RouteTemplateStops", new[] { "WarehouseId" });
            DropForeignKey("dbo.RouteTemplateStops", "TemplateId", "dbo.RouteTemplates");
            DropTable("dbo.RouteTemplateStops");
        }
    }
}

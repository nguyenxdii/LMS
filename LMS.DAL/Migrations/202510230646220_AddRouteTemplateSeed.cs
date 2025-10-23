namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRouteTemplateSeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RouteTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        FromWarehouseId = c.Int(nullable: false),
                        ToWarehouseId = c.Int(nullable: false),
                        DistanceKm = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.FromWarehouseId)
                .ForeignKey("dbo.Warehouses", t => t.ToWarehouseId)
                .Index(t => t.FromWarehouseId)
                .Index(t => t.ToWarehouseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteTemplates", "ToWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.RouteTemplates", "FromWarehouseId", "dbo.Warehouses");
            DropIndex("dbo.RouteTemplates", new[] { "ToWarehouseId" });
            DropIndex("dbo.RouteTemplates", new[] { "FromWarehouseId" });
            DropTable("dbo.RouteTemplates");
        }
    }
}

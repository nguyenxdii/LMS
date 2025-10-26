namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddStopsNavigationToRouteTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RouteTemplateStops", "RouteTemplate_Id", c => c.Int());
            //CreateIndex("dbo.RouteTemplateStops", "WarehouseId");
            CreateIndex("dbo.RouteTemplateStops", "RouteTemplate_Id");
            AddForeignKey("dbo.RouteTemplateStops", "WarehouseId", "dbo.Warehouses", "Id");
            AddForeignKey("dbo.RouteTemplateStops", "RouteTemplate_Id", "dbo.RouteTemplates", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.RouteTemplateStops", "RouteTemplate_Id", "dbo.RouteTemplates");
            DropForeignKey("dbo.RouteTemplateStops", "WarehouseId", "dbo.Warehouses");
            DropIndex("dbo.RouteTemplateStops", new[] { "RouteTemplate_Id" });
            DropIndex("dbo.RouteTemplateStops", new[] { "WarehouseId" });
            DropColumn("dbo.RouteTemplateStops", "RouteTemplate_Id");
        }
    }
}

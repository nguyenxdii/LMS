namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Sync_DriverFlow_Models : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Shipments", new[] { "DriverId" });
            DropIndex("dbo.RouteStops", new[] { "WarehouseId" });
            AddColumn("dbo.Shipments", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "VehicleId", c => c.Int());
            AddColumn("dbo.Shipments", "FromWarehouseId", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "ToWarehouseId", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.Shipments", "Note", c => c.String());
            AddColumn("dbo.Shipments", "CurrentStopSeq", c => c.Int());
            AddColumn("dbo.RouteStops", "StopName", c => c.String(maxLength: 200));
            AddColumn("dbo.RouteStops", "Seq", c => c.Int(nullable: false));
            AddColumn("dbo.RouteStops", "PlannedETA", c => c.DateTime());
            AddColumn("dbo.Orders", "OrderNo", c => c.String(maxLength: 50));
            AlterColumn("dbo.Shipments", "DriverId", c => c.Int(nullable: false));
            AlterColumn("dbo.RouteStops", "WarehouseId", c => c.Int());
            //AlterColumn("dbo.Orders", "ShipmentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "ShipmentId", c => c.Int());
            CreateIndex("dbo.Shipments", "OrderId");
            CreateIndex("dbo.Shipments", "DriverId");
            CreateIndex("dbo.Shipments", "VehicleId");
            CreateIndex("dbo.Shipments", "FromWarehouseId");
            CreateIndex("dbo.Shipments", "ToWarehouseId");
            CreateIndex("dbo.RouteStops", "WarehouseId");
            AddForeignKey("dbo.Shipments", "FromWarehouseId", "dbo.Warehouses", "Id");
            AddForeignKey("dbo.Shipments", "OrderId", "dbo.Orders", "Id");
            AddForeignKey("dbo.Shipments", "ToWarehouseId", "dbo.Warehouses", "Id");
            AddForeignKey("dbo.Shipments", "VehicleId", "dbo.Vehicles", "Id");
            DropColumn("dbo.Shipments", "CreatedAt");
            DropColumn("dbo.RouteStops", "Sequence");
            DropColumn("dbo.RouteStops", "IsFinal");
        }

        public override void Down()
        {
            AddColumn("dbo.RouteStops", "IsFinal", c => c.Boolean(nullable: false));
            AddColumn("dbo.RouteStops", "Sequence", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "CreatedAt", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Shipments", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Shipments", "ToWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Shipments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Shipments", "FromWarehouseId", "dbo.Warehouses");
            DropIndex("dbo.RouteStops", new[] { "WarehouseId" });
            DropIndex("dbo.Shipments", new[] { "ToWarehouseId" });
            DropIndex("dbo.Shipments", new[] { "FromWarehouseId" });
            DropIndex("dbo.Shipments", new[] { "VehicleId" });
            DropIndex("dbo.Shipments", new[] { "DriverId" });
            DropIndex("dbo.Shipments", new[] { "OrderId" });
            AlterColumn("dbo.Orders", "ShipmentId", c => c.Int());
            AlterColumn("dbo.RouteStops", "WarehouseId", c => c.Int(nullable: false));
            AlterColumn("dbo.Shipments", "DriverId", c => c.Int());
            DropColumn("dbo.Orders", "OrderNo");
            DropColumn("dbo.RouteStops", "PlannedETA");
            DropColumn("dbo.RouteStops", "Seq");
            DropColumn("dbo.RouteStops", "StopName");
            DropColumn("dbo.Shipments", "CurrentStopSeq");
            DropColumn("dbo.Shipments", "Note");
            DropColumn("dbo.Shipments", "UpdatedAt");
            DropColumn("dbo.Shipments", "ToWarehouseId");
            DropColumn("dbo.Shipments", "FromWarehouseId");
            DropColumn("dbo.Shipments", "VehicleId");
            DropColumn("dbo.Shipments", "OrderId");
            CreateIndex("dbo.RouteStops", "WarehouseId");
            CreateIndex("dbo.Shipments", "DriverId");
        }
    }
}

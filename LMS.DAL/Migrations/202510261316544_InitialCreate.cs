namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Phone = c.String(maxLength: 15),
                        Email = c.String(maxLength: 120),
                        Address = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false, maxLength: 256),
                        Role = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        DriverId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastLoginAt = c.DateTime(),
                        LastPasswordChangeAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.Username, unique: true)
                .Index(t => t.CustomerId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 120),
                        Phone = c.String(nullable: false, maxLength: 15),
                        CitizenId = c.String(maxLength: 12),
                        LicenseType = c.String(nullable: false, maxLength: 10),
                        IsActive = c.Boolean(nullable: false),
                        VehicleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        DriverId = c.Int(),
                        VehicleId = c.Int(),
                        FromWarehouseId = c.Int(nullable: false),
                        ToWarehouseId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        StartedAt = c.DateTime(),
                        DeliveredAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        Note = c.String(),
                        CurrentStopSeq = c.Int(),
                        ShipmentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.Warehouses", t => t.FromWarehouseId)
                .ForeignKey("dbo.Orders", t => t.ShipmentId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Warehouses", t => t.ToWarehouseId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .Index(t => t.OrderId)
                .Index(t => t.DriverId)
                .Index(t => t.VehicleId)
                .Index(t => t.FromWarehouseId)
                .Index(t => t.ToWarehouseId)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Address = c.String(nullable: false, maxLength: 200),
                        Lat = c.Double(),
                        Lng = c.Double(),
                        ZoneId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OriginWarehouseId = c.Int(nullable: false),
                        DestWarehouseId = c.Int(nullable: false),
                        NeedPickup = c.Boolean(nullable: false),
                        PickupAddress = c.String(maxLength: 200),
                        PackageDescription = c.String(maxLength: 200),
                        DesiredTime = c.DateTime(),
                        RouteFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PickupFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepositPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepositAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedAt = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        ShipmentId = c.Int(),
                        OrderNo = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Warehouses", t => t.DestWarehouseId)
                .ForeignKey("dbo.Warehouses", t => t.OriginWarehouseId)
                .Index(t => t.CustomerId)
                .Index(t => t.OriginWarehouseId)
                .Index(t => t.DestWarehouseId);
            
            CreateTable(
                "dbo.RouteStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShipmentId = c.Int(nullable: false),
                        WarehouseId = c.Int(),
                        StopName = c.String(maxLength: 200),
                        Seq = c.Int(nullable: false),
                        PlannedETA = c.DateTime(),
                        ArrivedAt = c.DateTime(),
                        DepartedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shipments", t => t.ShipmentId, cascadeDelete: true)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.ShipmentId)
                .Index(t => t.WarehouseId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlateNo = c.String(nullable: false, maxLength: 15),
                        Type = c.String(nullable: false, maxLength: 50),
                        CapacityKg = c.Int(),
                        VolumeM3 = c.Decimal(precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        LastMaintenanceDate = c.DateTime(),
                        Notes = c.String(maxLength: 500),
                        Driver_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.Driver_Id)
                .Index(t => t.PlateNo, unique: true)
                .Index(t => t.Driver_Id);
            
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
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .ForeignKey("dbo.RouteTemplates", t => t.TemplateId)
                .Index(t => t.TemplateId)
                .Index(t => t.WarehouseId);
            
            CreateTable(
                "dbo.ShipmentDriverLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShipmentId = c.Int(nullable: false),
                        OldDriverId = c.Int(),
                        NewDriverId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        StopSequenceNumber = c.Int(),
                        Reason = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.NewDriverId)
                .ForeignKey("dbo.Drivers", t => t.OldDriverId)
                .ForeignKey("dbo.Shipments", t => t.ShipmentId)
                .Index(t => t.ShipmentId)
                .Index(t => t.OldDriverId)
                .Index(t => t.NewDriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipmentDriverLogs", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentDriverLogs", "OldDriverId", "dbo.Drivers");
            DropForeignKey("dbo.ShipmentDriverLogs", "NewDriverId", "dbo.Drivers");
            DropForeignKey("dbo.RouteTemplates", "ToWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.RouteTemplateStops", "TemplateId", "dbo.RouteTemplates");
            DropForeignKey("dbo.RouteTemplateStops", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.RouteTemplates", "FromWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Vehicles", "Driver_Id", "dbo.Drivers");
            DropForeignKey("dbo.Shipments", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Shipments", "ToWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.RouteStops", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.RouteStops", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Shipments", "ShipmentId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OriginWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Orders", "DestWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Shipments", "FromWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Shipments", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.UserAccounts", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.UserAccounts", "CustomerId", "dbo.Customers");
            DropIndex("dbo.ShipmentDriverLogs", new[] { "NewDriverId" });
            DropIndex("dbo.ShipmentDriverLogs", new[] { "OldDriverId" });
            DropIndex("dbo.ShipmentDriverLogs", new[] { "ShipmentId" });
            DropIndex("dbo.RouteTemplateStops", new[] { "WarehouseId" });
            DropIndex("dbo.RouteTemplateStops", new[] { "TemplateId" });
            DropIndex("dbo.RouteTemplates", new[] { "ToWarehouseId" });
            DropIndex("dbo.RouteTemplates", new[] { "FromWarehouseId" });
            DropIndex("dbo.Vehicles", new[] { "Driver_Id" });
            DropIndex("dbo.Vehicles", new[] { "PlateNo" });
            DropIndex("dbo.RouteStops", new[] { "WarehouseId" });
            DropIndex("dbo.RouteStops", new[] { "ShipmentId" });
            DropIndex("dbo.Orders", new[] { "DestWarehouseId" });
            DropIndex("dbo.Orders", new[] { "OriginWarehouseId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Shipments", new[] { "ShipmentId" });
            DropIndex("dbo.Shipments", new[] { "ToWarehouseId" });
            DropIndex("dbo.Shipments", new[] { "FromWarehouseId" });
            DropIndex("dbo.Shipments", new[] { "VehicleId" });
            DropIndex("dbo.Shipments", new[] { "DriverId" });
            DropIndex("dbo.Shipments", new[] { "OrderId" });
            DropIndex("dbo.UserAccounts", new[] { "DriverId" });
            DropIndex("dbo.UserAccounts", new[] { "CustomerId" });
            DropIndex("dbo.UserAccounts", new[] { "Username" });
            DropTable("dbo.ShipmentDriverLogs");
            DropTable("dbo.RouteTemplateStops");
            DropTable("dbo.RouteTemplates");
            DropTable("dbo.Vehicles");
            DropTable("dbo.RouteStops");
            DropTable("dbo.Orders");
            DropTable("dbo.Warehouses");
            DropTable("dbo.Shipments");
            DropTable("dbo.Drivers");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Customers");
        }
    }
}

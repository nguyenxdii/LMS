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
                        LicenseType = c.String(nullable: false, maxLength: 10),
                        IsActive = c.Boolean(nullable: false),
                        VehicleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DriverId = c.Int(),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        StartedAt = c.DateTime(),
                        DeliveredAt = c.DateTime(),
                        ShipmentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.Orders", t => t.ShipmentId)
                .Index(t => t.DriverId)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.RouteStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShipmentId = c.Int(nullable: false),
                        WarehouseId = c.Int(nullable: false),
                        Sequence = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ArrivedAt = c.DateTime(),
                        DepartedAt = c.DateTime(),
                        IsFinal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shipments", t => t.ShipmentId, cascadeDelete: true)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.ShipmentId)
                .Index(t => t.WarehouseId);
            
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlateNo = c.String(nullable: false, maxLength: 15),
                        Model = c.String(maxLength: 50),
                        CapacityKg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VolumeM3 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsAvailable = c.Boolean(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Warehouses", t => t.DestWarehouseId)
                .ForeignKey("dbo.Warehouses", t => t.OriginWarehouseId)
                .Index(t => t.CustomerId)
                .Index(t => t.OriginWarehouseId)
                .Index(t => t.DestWarehouseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "ShipmentId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OriginWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Orders", "DestWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Drivers", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.RouteStops", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.RouteStops", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.UserAccounts", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.UserAccounts", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "DestWarehouseId" });
            DropIndex("dbo.Orders", new[] { "OriginWarehouseId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.RouteStops", new[] { "WarehouseId" });
            DropIndex("dbo.RouteStops", new[] { "ShipmentId" });
            DropIndex("dbo.Shipments", new[] { "ShipmentId" });
            DropIndex("dbo.Shipments", new[] { "DriverId" });
            DropIndex("dbo.Drivers", new[] { "VehicleId" });
            DropIndex("dbo.UserAccounts", new[] { "DriverId" });
            DropIndex("dbo.UserAccounts", new[] { "CustomerId" });
            DropIndex("dbo.UserAccounts", new[] { "Username" });
            DropTable("dbo.Orders");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Warehouses");
            DropTable("dbo.RouteStops");
            DropTable("dbo.Shipments");
            DropTable("dbo.Drivers");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Customers");
        }
    }
}

namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_OrderShipmentNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "ShipmentId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "ShipmentId", c => c.Int(nullable: false));
        }
    }
}

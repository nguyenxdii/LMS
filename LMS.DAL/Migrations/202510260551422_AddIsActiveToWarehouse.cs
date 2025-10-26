namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsActiveToWarehouse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Warehouses", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Warehouses", "IsActive");
        }
    }
}

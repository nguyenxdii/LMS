namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvaterToCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "AvatarData", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "AvatarData");
        }
    }
}

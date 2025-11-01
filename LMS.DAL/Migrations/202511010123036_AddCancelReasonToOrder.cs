namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelReasonToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CancelReason", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CancelReason");
        }
    }
}

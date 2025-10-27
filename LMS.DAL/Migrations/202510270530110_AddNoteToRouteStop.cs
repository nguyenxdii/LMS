namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoteToRouteStop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RouteStops", "Note", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RouteStops", "Note");
        }
    }
}

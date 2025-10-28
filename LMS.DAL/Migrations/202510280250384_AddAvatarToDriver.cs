namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvatarToDriver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "AvatarData", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "AvatarData");
        }
    }
}

namespace LMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Fix_OrderStatus_RemoveInTransit : DbMigration
    {
        public override void Up()
        {
            // Map mọi giá trị 2 (InTransit cũ) -> 1 (Approved)
            Sql("UPDATE Orders SET Status = 1 WHERE Status = 2");
        }

        public override void Down()
        {
        }
    }
}

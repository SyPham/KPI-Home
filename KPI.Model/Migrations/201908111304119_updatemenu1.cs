namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class updatemenu1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "UserID", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Resources", "UserID");
        }
    }
}

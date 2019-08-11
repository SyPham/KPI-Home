namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class updatemenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "ParentID", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Menus", "ParentID");
        }
    }
}

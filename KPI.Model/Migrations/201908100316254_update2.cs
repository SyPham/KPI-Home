namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Link", c => c.String());
            AddColumn("dbo.Menus", "FontAwesome", c => c.String());
            AddColumn("dbo.Menus", "BackgroudColor", c => c.String());
            AddColumn("dbo.Menus", "Permission", c => c.Int(nullable: false));
            AddColumn("dbo.Resources", "Menu", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "Menu");
            DropColumn("dbo.Menus", "Permission");
            DropColumn("dbo.Menus", "BackgroudColor");
            DropColumn("dbo.Menus", "FontAwesome");
            DropColumn("dbo.Menus", "Link");
        }
    }
}

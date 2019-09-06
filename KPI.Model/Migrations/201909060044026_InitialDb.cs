namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notifications", "Tag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Tag");
            DropColumn("dbo.Notifications", "CreateTime");
        }
    }
}

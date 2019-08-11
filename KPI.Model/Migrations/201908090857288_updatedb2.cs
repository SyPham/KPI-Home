namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "CheckRole", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "CheckRole");
        }
    }
}

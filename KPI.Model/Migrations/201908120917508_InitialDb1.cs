namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KPILevels", "Standard", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KPILevels", "Standard");
        }
    }
}

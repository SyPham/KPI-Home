namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTableKPI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KPIs", "Unit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KPIs", "Unit");
        }
    }
}

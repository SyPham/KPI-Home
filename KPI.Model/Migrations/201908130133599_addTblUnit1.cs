namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTblUnit1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KPIs", "Unit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KPIs", "Unit", c => c.String());
        }
    }
}

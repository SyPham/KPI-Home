namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateKpiLevel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.KPILevels", name: "Standard", newName: "WeeklyStandard");
            AddColumn("dbo.KPILevels", "MonthlyStandard", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "QuaterlyStandard", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "YearlyStandard", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KPILevels", "YearlyStandard");
            DropColumn("dbo.KPILevels", "QuaterlyStandard");
            DropColumn("dbo.KPILevels", "MonthlyStandard");
            RenameColumn(table: "dbo.KPILevels", name: "WeeklyStandard", newName: "Standard");
        }
    }
}

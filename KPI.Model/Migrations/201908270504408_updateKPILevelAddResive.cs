namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateKPILevelAddResive : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionPlans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ToDoName = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        SubmitDate = c.DateTime(nullable: false),
                        ApprovedBy = c.Int(nullable: false),
                        DelayDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Revises",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KPILevelCodePeriod = c.String(),
                        PeriodValue = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.KPILevels", "WeeklyTarget", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "MonthlyTarget", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "QuarterlyTarget", c => c.Int(nullable: false));
            AddColumn("dbo.KPILevels", "YearlyTarget", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KPILevels", "YearlyTarget");
            DropColumn("dbo.KPILevels", "QuarterlyTarget");
            DropColumn("dbo.KPILevels", "MonthlyTarget");
            DropColumn("dbo.KPILevels", "WeeklyTarget");
            DropTable("dbo.Revises");
            DropTable("dbo.ActionPlans");
        }
    }
}

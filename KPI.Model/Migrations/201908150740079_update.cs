namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Data", "Value", c => c.Int(nullable: false));
            AlterColumn("dbo.Data", "Week", c => c.Int(nullable: false));
            AlterColumn("dbo.Data", "Month", c => c.Int(nullable: false));
            AlterColumn("dbo.Data", "Quarter", c => c.Int(nullable: false));
            AlterColumn("dbo.Data", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Data", "Year", c => c.Int());
            AlterColumn("dbo.Data", "Quarter", c => c.Int());
            AlterColumn("dbo.Data", "Month", c => c.Int());
            AlterColumn("dbo.Data", "Week", c => c.Int());
            AlterColumn("dbo.Data", "Value", c => c.Int());
        }
    }
}

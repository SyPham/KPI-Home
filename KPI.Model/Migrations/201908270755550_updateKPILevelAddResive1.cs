namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateKPILevelAddResive1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ActionPlans", name: "ToDoName", newName: "Title");
            RenameColumn(table: "dbo.ActionPlans", name: "DueDate", newName: "Deadline");
            RenameColumn(table: "dbo.ActionPlans", name: "DelayDate", newName: "CreatedTime");
            AddColumn("dbo.ActionPlans", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.ActionPlans", "DataID", c => c.Int(nullable: false));
            AddColumn("dbo.ActionPlans", "CommentID", c => c.Int(nullable: false));
            AddColumn("dbo.ActionPlans", "KPILevelCodeAndPeriod", c => c.String());
            AddColumn("dbo.ActionPlans", "Description", c => c.String());
            AddColumn("dbo.ActionPlans", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.ActionPlans", "ApprovedStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActionPlans", "ApprovedStatus");
            DropColumn("dbo.ActionPlans", "Status");
            DropColumn("dbo.ActionPlans", "Description");
            DropColumn("dbo.ActionPlans", "KPILevelCodeAndPeriod");
            DropColumn("dbo.ActionPlans", "CommentID");
            DropColumn("dbo.ActionPlans", "DataID");
            DropColumn("dbo.ActionPlans", "UserID");
            RenameColumn(table: "dbo.ActionPlans", name: "CreatedTime", newName: "DelayDate");
            RenameColumn(table: "dbo.ActionPlans", name: "Deadline", newName: "DueDate");
            RenameColumn(table: "dbo.ActionPlans", name: "Title", newName: "ToDoName");
        }
    }
}

namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actionplanecate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionPlans", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.ActionPlans", "ActionPlanCategoryID", c => c.Int(nullable: false));
            DropColumn("dbo.ActionPlans", "CreatedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActionPlans", "CreatedTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.ActionPlans", "ActionPlanCategoryID");
            DropColumn("dbo.ActionPlans", "CreateTime");
        }
    }
}

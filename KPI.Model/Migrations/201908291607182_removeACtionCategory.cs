namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeACtionCategory : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ActionPlans", name: "Content", newName: "Tag");
            DropColumn("dbo.ActionPlans", "ActionPlanCategoryID");
            DropTable("dbo.ActionPlanCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ActionPlanCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ActionPlans", "ActionPlanCategoryID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.ActionPlans", name: "Tag", newName: "Content");
        }
    }
}

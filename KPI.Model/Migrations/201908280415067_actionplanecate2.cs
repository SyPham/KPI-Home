namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actionplanecate2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionPlanCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActionPlanCategories");
        }
    }
}

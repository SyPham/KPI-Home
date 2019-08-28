namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actionplanecate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionPlanCategories", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActionPlanCategories", "Title", c => c.Int(nullable: false));
        }
    }
}

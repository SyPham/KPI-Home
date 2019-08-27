namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateKPILevelAddResive2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionPlans", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActionPlans", "Content");
        }
    }
}

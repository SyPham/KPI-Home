namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "BackgroudColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "BackgroudColor");
        }
    }
}

namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecomment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "DataID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "DataID");
        }
    }
}

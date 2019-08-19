namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChartDetail2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentChartDetails", "Pings", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommentChartDetails", "Pings");
        }
    }
}

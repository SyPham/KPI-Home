namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChartDetail1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentChartDetails", "DataID", c => c.Int(nullable: false));
            AddColumn("dbo.CommentChartDetails", "UserID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommentChartDetails", "UserID");
            DropColumn("dbo.CommentChartDetails", "DataID");
        }
    }
}

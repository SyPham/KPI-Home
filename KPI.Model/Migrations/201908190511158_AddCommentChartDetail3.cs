namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChartDetail3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentChartDetails", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommentChartDetails", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommentChartDetails", "FileMimeType", c => c.String());
            AddColumn("dbo.CommentChartDetails", "File", c => c.String());
            AddColumn("dbo.CommentChartDetails", "FileURL", c => c.String());
            AddColumn("dbo.CommentChartDetails", "Item", c => c.Int(nullable: false));
            AddColumn("dbo.CommentChartDetails", "CreateByStaff", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommentChartDetails", "CreateByStaff");
            DropColumn("dbo.CommentChartDetails", "Item");
            DropColumn("dbo.CommentChartDetails", "FileURL");
            DropColumn("dbo.CommentChartDetails", "File");
            DropColumn("dbo.CommentChartDetails", "FileMimeType");
            DropColumn("dbo.CommentChartDetails", "Modified");
            DropColumn("dbo.CommentChartDetails", "Created");
        }
    }
}

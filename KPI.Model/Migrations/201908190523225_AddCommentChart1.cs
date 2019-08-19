namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChart1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentCharts", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommentCharts", "Creator", c => c.Int(nullable: false));
            AddColumn("dbo.CommentCharts", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommentCharts", "Item", c => c.Int(nullable: false));
            AddColumn("dbo.CommentCharts", "OriginalItemHistoryID", c => c.String());
            AddColumn("dbo.CommentCharts", "Parent", c => c.Int());
            AddColumn("dbo.CommentCharts", "Content", c => c.String());
            AddColumn("dbo.CommentCharts", "Pings", c => c.String());
            AddColumn("dbo.CommentCharts", "File", c => c.String());
            AddColumn("dbo.CommentCharts", "FileURL", c => c.String());
            AddColumn("dbo.CommentCharts", "FileMimeType", c => c.String());
            AddColumn("dbo.CommentCharts", "FullName", c => c.String());
            AddColumn("dbo.CommentCharts", "ProfilePictureUrl", c => c.String());
            AddColumn("dbo.CommentCharts", "CreateByStaff", c => c.String());
            AddColumn("dbo.CommentCharts", "CreatedByCurrentUser", c => c.Boolean(nullable: false));
            AddColumn("dbo.CommentCharts", "UpvoteCount", c => c.Int(nullable: false));
            AddColumn("dbo.CommentCharts", "UserHasUpvoted", c => c.Boolean(nullable: false));
            AddColumn("dbo.CommentCharts", "UserID", c => c.Int(nullable: false));
            DropColumn("dbo.CommentCharts", "CommentDetailID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommentCharts", "CommentDetailID", c => c.Int(nullable: false));
            DropColumn("dbo.CommentCharts", "UserID");
            DropColumn("dbo.CommentCharts", "UserHasUpvoted");
            DropColumn("dbo.CommentCharts", "UpvoteCount");
            DropColumn("dbo.CommentCharts", "CreatedByCurrentUser");
            DropColumn("dbo.CommentCharts", "CreateByStaff");
            DropColumn("dbo.CommentCharts", "ProfilePictureUrl");
            DropColumn("dbo.CommentCharts", "FullName");
            DropColumn("dbo.CommentCharts", "FileMimeType");
            DropColumn("dbo.CommentCharts", "FileURL");
            DropColumn("dbo.CommentCharts", "File");
            DropColumn("dbo.CommentCharts", "Pings");
            DropColumn("dbo.CommentCharts", "Content");
            DropColumn("dbo.CommentCharts", "Parent");
            DropColumn("dbo.CommentCharts", "OriginalItemHistoryID");
            DropColumn("dbo.CommentCharts", "Item");
            DropColumn("dbo.CommentCharts", "Modified");
            DropColumn("dbo.CommentCharts", "Creator");
            DropColumn("dbo.CommentCharts", "Created");
        }
    }
}

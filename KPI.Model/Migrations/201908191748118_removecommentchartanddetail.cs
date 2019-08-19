namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removecommentchartanddetail : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.CommentChartDetails");
            DropTable("dbo.CommentCharts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CommentCharts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Creator = c.Int(nullable: false),
                        Modified = c.DateTime(nullable: false),
                        Item = c.Int(nullable: false),
                        OriginalItemHistoryID = c.String(),
                        Parent = c.Int(nullable: false),
                        Content = c.String(),
                        Pings = c.String(),
                        File = c.String(),
                        FileURL = c.String(),
                        FileMimeType = c.String(),
                        FullName = c.String(),
                        ProfilePictureUrl = c.String(),
                        CreateByStaff = c.Boolean(nullable: false),
                        CreatedByCurrentUser = c.Boolean(nullable: false),
                        UpvoteCount = c.Int(nullable: false),
                        UserHasUpvoted = c.Boolean(nullable: false),
                        DataID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CommentChartDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DataID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Parent = c.Int(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(nullable: false),
                        FileMimeType = c.String(),
                        Content = c.String(),
                        Pings = c.String(),
                        FullName = c.String(),
                        ProfilePictureUrl = c.String(),
                        Creator = c.Int(nullable: false),
                        CreatedByAdmin = c.Boolean(nullable: false),
                        CreatedByCurrentUser = c.Boolean(nullable: false),
                        File = c.String(),
                        FileURL = c.String(),
                        Item = c.Int(nullable: false),
                        CreateByStaff = c.String(),
                        UpvoteCount = c.Int(nullable: false),
                        userHasUpvoted = c.Boolean(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}

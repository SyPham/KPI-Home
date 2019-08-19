namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChartDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentChartDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Parent = c.Int(),
                        Content = c.String(),
                        FullName = c.String(),
                        ProfilePictureUrl = c.String(),
                        Creator = c.Int(nullable: false),
                        CreatedByAdmin = c.Boolean(nullable: false),
                        CreatedByCurrentUser = c.Boolean(nullable: false),
                        UpvoteCount = c.Int(nullable: false),
                        userHasUpvoted = c.Boolean(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommentChartDetails");
        }
    }
}

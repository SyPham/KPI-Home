namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentCharts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CommentDetailID = c.Int(nullable: false),
                        DataID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommentCharts");
        }
    }
}

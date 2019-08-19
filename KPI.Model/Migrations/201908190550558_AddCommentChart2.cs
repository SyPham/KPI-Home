namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChart2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CommentCharts", "Parent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CommentCharts", "Parent", c => c.Int());
        }
    }
}

namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentChart3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CommentCharts", "CreateByStaff", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CommentCharts", "CreateByStaff", c => c.String());
        }
    }
}

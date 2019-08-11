namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRoleandCredential1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "Code");
        }
    }
}

namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSkype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Skype", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Skype");
        }
    }
}

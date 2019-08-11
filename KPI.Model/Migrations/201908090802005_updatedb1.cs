namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "FontAwesome", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "FontAwesome");
        }
    }
}

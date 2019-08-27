namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermison : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "URLDefault", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Permissions", "URLDefault");
        }
    }
}

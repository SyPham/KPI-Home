namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Data", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Data", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}

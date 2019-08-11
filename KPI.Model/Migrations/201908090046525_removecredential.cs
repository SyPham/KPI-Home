namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removecredential : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Credentials");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Credentials",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}

namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PermissionName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        Name = c.String(),
                        State = c.Boolean(nullable: false),
                        Permission = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Users", "Permission", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Permission");
            DropTable("dbo.Resources");
            DropTable("dbo.Permissions");
        }
    }
}

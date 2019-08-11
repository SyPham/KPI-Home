namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class version : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KPILevels", "Weekly", c => c.Int());
        }

        public override void Down()
        {
            AlterColumn("dbo.KPILevels", "Weekly", c => c.String());
        }
    }
}

namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateData : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Data", name: "KPIKind", newName: "Period");
            RenameColumn(table: "dbo.Data", name: "Quater", newName: "Quarter");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Data", name: "Quarter", newName: "Quater");
            RenameColumn(table: "dbo.Data", name: "Period", newName: "KPIKind");
        }
    }
}

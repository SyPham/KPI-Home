namespace KPI.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateKpiLevel2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.KPILevels", name: "Quaterly", newName: "Quarterly");
            RenameColumn(table: "dbo.KPILevels", name: "QuaterlyChecked", newName: "QuarterlyChecked");
            RenameColumn(table: "dbo.KPILevels", name: "QuaterlyPublic", newName: "QuarterlyPublic");
            RenameColumn(table: "dbo.KPILevels", name: "QuaterlyStandard", newName: "QuarterlyStandard");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.KPILevels", name: "QuarterlyStandard", newName: "QuaterlyStandard");
            RenameColumn(table: "dbo.KPILevels", name: "QuarterlyPublic", newName: "QuaterlyPublic");
            RenameColumn(table: "dbo.KPILevels", name: "QuarterlyChecked", newName: "QuaterlyChecked");
            RenameColumn(table: "dbo.KPILevels", name: "Quarterly", newName: "Quaterly");
        }
    }
}

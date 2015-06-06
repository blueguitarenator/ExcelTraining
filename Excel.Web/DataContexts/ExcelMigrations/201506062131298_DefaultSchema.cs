namespace Excel.Web.DataContexts.ExcelMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultSchema : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Athletes", newSchema: "athletes");
            MoveTable(name: "dbo.Trainers", newSchema: "athletes");
        }
        
        public override void Down()
        {
            MoveTable(name: "athletes.Trainers", newSchema: "dbo");
            MoveTable(name: "athletes.Athletes", newSchema: "dbo");
        }
    }
}

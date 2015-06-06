namespace Excel.Web.DataContexts.ExcelMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAthleteType : DbMigration
    {
        public override void Up()
        {
            AddColumn("athletes.Athletes", "AthleteType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("athletes.Athletes", "AthleteType");
        }
    }
}

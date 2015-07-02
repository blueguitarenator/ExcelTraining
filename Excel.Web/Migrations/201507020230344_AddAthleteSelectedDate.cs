namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAthleteSelectedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.Athletes", "SelectedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("identity.Athletes", "SelectedDate");
        }
    }
}

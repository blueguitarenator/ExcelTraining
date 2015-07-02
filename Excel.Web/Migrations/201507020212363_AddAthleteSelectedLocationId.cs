namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAthleteSelectedLocationId : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.Athletes", "SelectedLocationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("identity.Athletes", "SelectedLocationId");
        }
    }
}

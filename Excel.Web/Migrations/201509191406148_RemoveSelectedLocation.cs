namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSelectedLocation : DbMigration
    {
        public override void Up()
        {
            DropColumn("identity.Athletes", "SelectedLocationId");

        }
        
        public override void Down()
        {
            AddColumn("identity.Athletes", "SelectedLocationId", c => c.Int(nullable: false));
        }
    }
}

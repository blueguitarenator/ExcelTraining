namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationToSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.Sessions", "LocationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("identity.Sessions", "LocationId");
        }
    }
}

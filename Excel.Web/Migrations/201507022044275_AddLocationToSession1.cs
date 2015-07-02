namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationToSession1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("identity.Sessions", "LocationId");
            AddForeignKey("identity.Sessions", "LocationId", "identity.Locations", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("identity.Sessions", "LocationId", "identity.Locations");
            DropIndex("identity.Sessions", new[] { "LocationId" });
        }
    }
}

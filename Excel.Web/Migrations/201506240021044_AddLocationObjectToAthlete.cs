namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationObjectToAthlete : DbMigration
    {
        public override void Up()
        {
            CreateIndex("identity.Athletes", "LocationId");
            AddForeignKey("identity.Athletes", "LocationId", "identity.Locations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("identity.Athletes", "LocationId", "identity.Locations");
            DropIndex("identity.Athletes", new[] { "LocationId" });
        }
    }
}

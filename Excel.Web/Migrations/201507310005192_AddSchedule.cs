namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "identity.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hour = c.Int(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        AthleteType = c.Int(nullable: false),
                        Location_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.Locations", t => t.Location_Id, cascadeDelete: true)
                .Index(t => t.Location_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("identity.Schedules", "Location_Id", "identity.Locations");
            DropIndex("identity.Schedules", new[] { "Location_Id" });
            DropTable("identity.Schedules");
        }
    }
}

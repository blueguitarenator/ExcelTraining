namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "identity.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("identity.Athletes", "location_Id", c => c.Int());
            CreateIndex("identity.Athletes", "location_Id");
            AddForeignKey("identity.Athletes", "location_Id", "identity.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("identity.Athletes", "location_Id", "identity.Locations");
            DropIndex("identity.Athletes", new[] { "location_Id" });
            DropColumn("identity.Athletes", "location_Id");
            DropTable("identity.Locations");
        }
    }
}

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
            
            AddColumn("identity.Athletes", "LocationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("identity.Athletes", "LocationId");
            DropTable("identity.Locations");
        }
    }
}

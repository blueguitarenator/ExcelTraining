namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "identity.CellPhoneCarriers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("identity.Athletes", "CellNumber", c => c.String(maxLength: 12));
            AddColumn("identity.Athletes", "CellPhoneCarrierId", c => c.Int());
            AddColumn("identity.Athletes", "BirthDate", c => c.DateTime(nullable: false));
            CreateIndex("identity.Athletes", "CellPhoneCarrierId");
            AddForeignKey("identity.Athletes", "CellPhoneCarrierId", "identity.CellPhoneCarriers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("identity.Athletes", "CellPhoneCarrierId", "identity.CellPhoneCarriers");
            DropIndex("identity.Athletes", new[] { "CellPhoneCarrierId" });
            DropColumn("identity.Athletes", "BirthDate");
            DropColumn("identity.Athletes", "CellPhoneCarrierId");
            DropColumn("identity.Athletes", "CellNumber");
            DropTable("identity.CellPhoneCarriers");
        }
    }
}

namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Motd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "identity.Motds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpireDateTime = c.DateTime(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("identity.Motds");
        }
    }
}

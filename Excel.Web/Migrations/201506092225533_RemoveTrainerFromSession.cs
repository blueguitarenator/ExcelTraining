namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTrainerFromSession : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("identity.Sessions", "Trainer_Id", "identity.Trainers");
            DropIndex("identity.Sessions", new[] { "Trainer_Id" });
            DropColumn("identity.Sessions", "Trainer_Id");
            DropTable("identity.Trainers");
        }
        
        public override void Down()
        {
            CreateTable(
                "identity.Trainers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                        City = c.String(nullable: false, maxLength: 255),
                        State = c.String(nullable: false, maxLength: 255),
                        Zip = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("identity.Sessions", "Trainer_Id", c => c.Int());
            CreateIndex("identity.Sessions", "Trainer_Id");
            AddForeignKey("identity.Sessions", "Trainer_Id", "identity.Trainers", "Id");
        }
    }
}

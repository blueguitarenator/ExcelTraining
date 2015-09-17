namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InjuryNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "identity.InjuryNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NoteDate = c.DateTime(nullable: false),
                        Message = c.String(nullable: false),
                        Athlete_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.Athletes", t => t.Athlete_Id)
                .Index(t => t.Athlete_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("identity.InjuryNotes", "Athlete_Id", "identity.Athletes");
            DropIndex("identity.InjuryNotes", new[] { "Athlete_Id" });
            DropTable("identity.InjuryNotes");
        }
    }
}

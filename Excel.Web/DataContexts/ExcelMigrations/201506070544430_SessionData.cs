namespace Excel.Web.DataContexts.ExcelMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "athletes.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hour = c.Int(nullable: false),
                        Day = c.DateTime(nullable: false),
                        Trainer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("athletes.Trainers", t => t.Trainer_Id)
                .Index(t => t.Trainer_Id);
            
            CreateTable(
                "athletes.SessionAthletes",
                c => new
                    {
                        Session_Id = c.Int(nullable: false),
                        Athlete_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Session_Id, t.Athlete_Id })
                .ForeignKey("athletes.Sessions", t => t.Session_Id, cascadeDelete: true)
                .ForeignKey("athletes.Athletes", t => t.Athlete_Id, cascadeDelete: true)
                .Index(t => t.Session_Id)
                .Index(t => t.Athlete_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("athletes.Sessions", "Trainer_Id", "athletes.Trainers");
            DropForeignKey("athletes.SessionAthletes", "Athlete_Id", "athletes.Athletes");
            DropForeignKey("athletes.SessionAthletes", "Session_Id", "athletes.Sessions");
            DropIndex("athletes.SessionAthletes", new[] { "Athlete_Id" });
            DropIndex("athletes.SessionAthletes", new[] { "Session_Id" });
            DropIndex("athletes.Sessions", new[] { "Trainer_Id" });
            DropTable("athletes.SessionAthletes");
            DropTable("athletes.Sessions");
        }
    }
}

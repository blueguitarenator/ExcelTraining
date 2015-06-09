namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "identity.Athletes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                        City = c.String(nullable: false, maxLength: 255),
                        State = c.String(nullable: false, maxLength: 255),
                        Zip = c.String(nullable: false, maxLength: 255),
                        AthleteType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "identity.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hour = c.Int(nullable: false),
                        Day = c.DateTime(nullable: false),
                        Trainer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.Trainers", t => t.Trainer_Id)
                .Index(t => t.Trainer_Id);
            
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
            
            CreateTable(
                "identity.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "identity.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("identity.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("identity.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "identity.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Athlete_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.Athletes", t => t.Athlete_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Athlete_Id);
            
            CreateTable(
                "identity.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "identity.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("identity.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "identity.SessionAthletes",
                c => new
                    {
                        Session_Id = c.Int(nullable: false),
                        Athlete_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Session_Id, t.Athlete_Id })
                .ForeignKey("identity.Sessions", t => t.Session_Id, cascadeDelete: true)
                .ForeignKey("identity.Athletes", t => t.Athlete_Id, cascadeDelete: true)
                .Index(t => t.Session_Id)
                .Index(t => t.Athlete_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("identity.AspNetUserRoles", "UserId", "identity.AspNetUsers");
            DropForeignKey("identity.AspNetUserLogins", "UserId", "identity.AspNetUsers");
            DropForeignKey("identity.AspNetUserClaims", "UserId", "identity.AspNetUsers");
            DropForeignKey("identity.AspNetUsers", "Athlete_Id", "identity.Athletes");
            DropForeignKey("identity.AspNetUserRoles", "RoleId", "identity.AspNetRoles");
            DropForeignKey("identity.Sessions", "Trainer_Id", "identity.Trainers");
            DropForeignKey("identity.SessionAthletes", "Athlete_Id", "identity.Athletes");
            DropForeignKey("identity.SessionAthletes", "Session_Id", "identity.Sessions");
            DropIndex("identity.SessionAthletes", new[] { "Athlete_Id" });
            DropIndex("identity.SessionAthletes", new[] { "Session_Id" });
            DropIndex("identity.AspNetUserLogins", new[] { "UserId" });
            DropIndex("identity.AspNetUserClaims", new[] { "UserId" });
            DropIndex("identity.AspNetUsers", new[] { "Athlete_Id" });
            DropIndex("identity.AspNetUsers", "UserNameIndex");
            DropIndex("identity.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("identity.AspNetUserRoles", new[] { "UserId" });
            DropIndex("identity.AspNetRoles", "RoleNameIndex");
            DropIndex("identity.Sessions", new[] { "Trainer_Id" });
            DropTable("identity.SessionAthletes");
            DropTable("identity.AspNetUserLogins");
            DropTable("identity.AspNetUserClaims");
            DropTable("identity.AspNetUsers");
            DropTable("identity.AspNetUserRoles");
            DropTable("identity.AspNetRoles");
            DropTable("identity.Trainers");
            DropTable("identity.Sessions");
            DropTable("identity.Athletes");
        }
    }
}

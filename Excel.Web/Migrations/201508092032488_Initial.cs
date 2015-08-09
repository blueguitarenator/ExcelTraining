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
                        UserType = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        SelectedLocationId = c.Int(nullable: false),
                        SelectedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "identity.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "identity.SessionAthletes",
                c => new
                    {
                        SessionId = c.Int(nullable: false),
                        AthleteId = c.Int(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SessionId, t.AthleteId })
                .ForeignKey("identity.Athletes", t => t.AthleteId, cascadeDelete: true)
                .ForeignKey("identity.Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId)
                .Index(t => t.AthleteId);
            
            CreateTable(
                "identity.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        Hour = c.Int(nullable: false),
                        Day = c.DateTime(nullable: false),
                        AthleteType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.Locations", t => t.LocationId)
                .Index(t => t.LocationId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("identity.AspNetUserRoles", "UserId", "identity.AspNetUsers");
            DropForeignKey("identity.AspNetUserLogins", "UserId", "identity.AspNetUsers");
            DropForeignKey("identity.AspNetUserClaims", "UserId", "identity.AspNetUsers");
            DropForeignKey("identity.AspNetUsers", "Athlete_Id", "identity.Athletes");
            DropForeignKey("identity.Schedules", "Location_Id", "identity.Locations");
            DropForeignKey("identity.AspNetUserRoles", "RoleId", "identity.AspNetRoles");
            DropForeignKey("identity.SessionAthletes", "SessionId", "identity.Sessions");
            DropForeignKey("identity.Sessions", "LocationId", "identity.Locations");
            DropForeignKey("identity.SessionAthletes", "AthleteId", "identity.Athletes");
            DropForeignKey("identity.Athletes", "LocationId", "identity.Locations");
            DropIndex("identity.AspNetUserLogins", new[] { "UserId" });
            DropIndex("identity.AspNetUserClaims", new[] { "UserId" });
            DropIndex("identity.AspNetUsers", new[] { "Athlete_Id" });
            DropIndex("identity.AspNetUsers", "UserNameIndex");
            DropIndex("identity.Schedules", new[] { "Location_Id" });
            DropIndex("identity.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("identity.AspNetUserRoles", new[] { "UserId" });
            DropIndex("identity.AspNetRoles", "RoleNameIndex");
            DropIndex("identity.Sessions", new[] { "LocationId" });
            DropIndex("identity.SessionAthletes", new[] { "AthleteId" });
            DropIndex("identity.SessionAthletes", new[] { "SessionId" });
            DropIndex("identity.Athletes", new[] { "LocationId" });
            DropTable("identity.AspNetUserLogins");
            DropTable("identity.AspNetUserClaims");
            DropTable("identity.AspNetUsers");
            DropTable("identity.Schedules");
            DropTable("identity.AspNetUserRoles");
            DropTable("identity.AspNetRoles");
            DropTable("identity.Sessions");
            DropTable("identity.SessionAthletes");
            DropTable("identity.Locations");
            DropTable("identity.Athletes");
        }
    }
}

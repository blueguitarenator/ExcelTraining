namespace Excel.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAthleteToIdentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.AspNetUsers", "Athlete_Id", c => c.Int());
            CreateIndex("identity.AspNetUsers", "Athlete_Id");
            AddForeignKey("identity.AspNetUsers", "Athlete_Id", "athletes.Athletes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("identity.AspNetUsers", "Athlete_Id", "athletes.Athletes");
            DropIndex("identity.AspNetUsers", new[] { "Athlete_Id" });
            DropColumn("identity.AspNetUsers", "Athlete_Id");
        }
    }
}

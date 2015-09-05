namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AthleteHearAbout : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.Athletes", "HearAboutUsId", c => c.Int());
            CreateIndex("identity.Athletes", "HearAboutUsId");
            AddForeignKey("identity.Athletes", "HearAboutUsId", "identity.HearAboutUs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("identity.Athletes", "HearAboutUsId", "identity.HearAboutUs");
            DropIndex("identity.Athletes", new[] { "HearAboutUsId" });
            DropColumn("identity.Athletes", "HearAboutUsId");
        }
    }
}

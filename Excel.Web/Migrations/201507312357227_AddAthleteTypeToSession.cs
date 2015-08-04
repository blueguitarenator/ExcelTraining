namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAthleteTypeToSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.Sessions", "AthleteType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("identity.Sessions", "AthleteType");
        }
    }
}

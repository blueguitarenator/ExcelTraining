namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserType : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.Athletes", "UserType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("identity.Athletes", "UserType");
        }
    }
}

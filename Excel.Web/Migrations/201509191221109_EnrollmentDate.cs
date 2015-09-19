namespace Excel.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnrollmentDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("identity.Athletes", "EnrollmentDate", c => c.DateTime(nullable: false));
            DropColumn("identity.Athletes", "SelectedDate");
        }
        
        public override void Down()
        {
            AddColumn("identity.Athletes", "SelectedDate", c => c.DateTime(nullable: false));
            DropColumn("identity.Athletes", "EnrollmentDate");
        }
    }
}

namespace Excel.Web.Migrations
{
    using Excel.Entities;
    using Excel.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Excel.Web.DataContexts.IdentityDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Excel.Web.DataContexts.IdentityDb context)
        {
            if (!context.Users.Any(u => u.Email == "ray@msn.com"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var athlete = new Athlete { FirstName = "Ray", LastName = "Ramano", Address = "123 Main Street", City = "OFallon", State = "MO", Zip = "63366", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Trainer };
                var user = new ApplicationUser { Email = "ray@msn.com", UserName = "ray@msn.com" };

                manager.Create(user, "123434");
                roleManager.Create(new IdentityRole { Name = "admin" });
                manager.AddToRole(user.Id, "admin");
            }
        }
    }
}

namespace Excel.Web.Migrations
{
    using Excel.Entities;
    using Excel.Web.DataContexts;
    using Excel.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Excel.Web.DataContexts.IdentityDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(IdentityDb context)
        {
            if (!context.Users.Any(u => u.Email == "rich@msn.com"))
            {
                var dardenne = new Location { Name = "Dardenne Prairie" };
                var midrivers = new Location { Name = "Mid Rivers" };
                context.SaveChanges();
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var rich = new Athlete { FirstName = "Rich", LastName = "Schwepker", Address = "123 Main Street", City = "OFallon", State = "MO", Zip = "63366", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Trainer, Location = dardenne };
                var user = new ApplicationUser { Email = "rich@msn.com", UserName = "rich@msn.com", Athlete = rich };

                manager.Create(user, "123434");
                roleManager.Create(new IdentityRole { Name = "admin" });
                manager.AddToRole(user.Id, "admin");

                moreSeed(manager, context, midrivers, dardenne);
            }
        }

        private void moreSeed(UserManager<ApplicationUser> manager, IdentityDb context, Location midrivers, Location dardenne)
        {
            Athlete kenny = new Athlete { FirstName = "Kenny", LastName = "Ball", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne };
            ApplicationUser k = new ApplicationUser { Email = "kenny@msn.com", UserName = "kenny@msn.com", Athlete = kenny };
            manager.Create(k, "123434");
            manager.AddToRole(k.Id, "admin");

            Athlete george = new Athlete { FirstName = "George", LastName = "Harrison", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne };
            ApplicationUser g = new ApplicationUser { Email = "george@msn.com", UserName = "george@msn.com", Athlete = george };
            manager.Create(g, "123434");

            Athlete john = new Athlete { FirstName = "John", LastName = "Lennon", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = midrivers };
            ApplicationUser j = new ApplicationUser { Email = "john@msn.com", UserName = "john@msn.com", Athlete = john };
            manager.Create(j, "123434");

            Athlete paul = new Athlete { FirstName = "Paul", LastName = "McCartney", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne };
            ApplicationUser p = new ApplicationUser { Email = "paul@msn.com", UserName = "paul@msn.com", Athlete = paul };
            manager.Create(p, "123434");

            Athlete ringo = new Athlete { FirstName = "Ringo", LastName = "Starr", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne };
            ApplicationUser r = new ApplicationUser { Email = "ringo@msn.com", UserName = "ringo@msn.com", Athlete = ringo };
            manager.Create(r, "123434");

            var session6Athletes = new List<Athlete> { george, john, kenny};
            var session7Athletes = new List<Athlete> { john, paul, kenny};
            var session8Athletes = new List<Athlete> { george, ringo, kenny};
            DateTime saveNow = DateTime.Now;
            var sessions = new List<Session>
            {
                new Session{Hour =6, Day =saveNow, Athletes=session6Athletes},
                new Session{Hour =7, Day =saveNow, Athletes=session7Athletes},
                new Session{Hour =8, Day =saveNow, Athletes=session8Athletes},
                new Session{Hour =9, Day =saveNow, Athletes=session8Athletes},
                new Session{Hour =10, Day =saveNow, Athletes=session8Athletes},
                new Session{Hour =16, Day =saveNow, Athletes=session8Athletes},
                new Session{Hour =17, Day =saveNow, Athletes=session8Athletes},
                new Session{Hour =18, Day =saveNow, Athletes=session8Athletes},
            };
            sessions.ForEach(s => context.Sessions.AddOrUpdate(a => a.Hour, s));

            //var locationDardenne = new Location { Name = "Dardenne Prairie" };
            //var locationMidRivers = new Location { Name = "Mid Rivers" };

            SaveChanges(context);
            base.Seed(context);
        }

        private void SaveChanges(IdentityDb context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}

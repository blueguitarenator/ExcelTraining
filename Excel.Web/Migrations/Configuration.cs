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
                SaveChanges(context);
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var rich = new Athlete { FirstName = "Rich", LastName = "Schwepker", Address = "123 Main Street", City = "OFallon", State = "MO", Zip = "63366", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Trainer, Location = dardenne, EnrollmentDate = DateTime.Now.Date };
                var user = new ApplicationUser { Email = "rich@msn.com", UserName = "rich@msn.com", Athlete = rich };

                manager.Create(user, "123434");
                roleManager.Create(new IdentityRole { Name = "admin" });
                manager.AddToRole(user.Id, "admin");

                moreSeed(manager, context, midrivers, dardenne);
            }

        }

        private void moreSeed(UserManager<ApplicationUser> manager, IdentityDb context, Location midrivers, Location dardenne)
        {
            DateTime saveNow = DateTime.Now.Date;
            Athlete kenny = new Athlete { FirstName = "Kenny", LastName = "Ball", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, EnrollmentDate = saveNow };
            ApplicationUser k = new ApplicationUser { Email = "kenny@msn.com", UserName = "kenny@msn.com", Athlete = kenny };
            manager.Create(k, "123434");
            manager.AddToRole(k.Id, "admin");

            Athlete george = new Athlete { FirstName = "George", LastName = "Harrison", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne, EnrollmentDate = saveNow };
            ApplicationUser g = new ApplicationUser { Email = "george@msn.com", UserName = "george@msn.com", Athlete = george };
            manager.Create(g, "123434");

            Athlete john = new Athlete { FirstName = "John", LastName = "Lennon", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = midrivers, EnrollmentDate = saveNow };
            ApplicationUser j = new ApplicationUser { Email = "john@msn.com", UserName = "john@msn.com", Athlete = john };
            manager.Create(j, "123434");

            Athlete paul = new Athlete { FirstName = "Paul", LastName = "McCartney", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne, EnrollmentDate = saveNow };
            ApplicationUser p = new ApplicationUser { Email = "paul@msn.com", UserName = "paul@msn.com", Athlete = paul };
            manager.Create(p, "123434");

            Athlete ringo = new Athlete { FirstName = "Ringo", LastName = "Starr", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne, EnrollmentDate = saveNow };
            ApplicationUser r = new ApplicationUser { Email = "ringo@msn.com", UserName = "ringo@msn.com", Athlete = ringo };
            manager.Create(r, "123434");

            var session6Athletes = new List<Athlete> { george, john, kenny};
            var session7Athletes = new List<Athlete> { john, paul, kenny};
            var session8Athletes = new List<Athlete> { george, ringo, kenny};

            //var sessions = new List<Session>
            //{
            //    new Session{Hour =6, Day =saveNow, Athletes=session6Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =7, Day =saveNow, Athletes=session7Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =8, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =9, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =10, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =16, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =17, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =18, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //};
            //sessions.ForEach(s => context.Sessions.AddOrUpdate(a => a.Hour, s));
            //var sessionsSports = new List<Session>
            //{
            //    new Session{Hour =16, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =17, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =18, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =19, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =20, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //};
            //sessions.ForEach(s => context.Sessions.AddOrUpdate(a => a.Hour, s));

            var schedulesPersonalTraining = new List<Schedule>
            {
                new Schedule { Hour = 6, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 7, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 8, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 9, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 10, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 11, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 12, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 13, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 14, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 15, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 16, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 17, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 18, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 19, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 20, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 21, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining }
            };
            schedulesPersonalTraining.ForEach(s => context.Schedules.AddOrUpdate(a => a.Hour, s));

            var schedulesSportsTraining = new List<Schedule>
            {
                new Schedule { Hour = 6, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 7, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 8, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 9, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 10, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 11, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 12, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 13, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 14, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 15, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 16, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 17, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 18, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 19, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 20, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 21, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining }
            };
            schedulesSportsTraining.ForEach(s => context.Schedules.AddOrUpdate(a => a.Hour, s));

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

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
            AutomaticMigrationsEnabled = true;
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
                var rich = new Athlete { FirstName = "Rich", LastName = "Schwepker", Address = "123 Main Street", City = "OFallon", State = "MO", Zip = "63366", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = DateTime.Now.Date };
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
            Athlete kenny = new Athlete { FirstName = "Kenny", LastName = "Ball", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = saveNow };
            ApplicationUser k = new ApplicationUser { Email = "kenny@msn.com", UserName = "kenny@msn.com", Athlete = kenny };
            manager.Create(k, "123434");
            manager.AddToRole(k.Id, "admin");

            Athlete george = new Athlete { FirstName = "George", LastName = "Harrison", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = saveNow };
            ApplicationUser g = new ApplicationUser { Email = "george@msn.com", UserName = "george@msn.com", Athlete = george };
            manager.Create(g, "123434");

            Athlete john = new Athlete { FirstName = "John", LastName = "Lennon", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = midrivers, SelectedDate = saveNow };
            ApplicationUser j = new ApplicationUser { Email = "john@msn.com", UserName = "john@msn.com", Athlete = john };
            manager.Create(j, "123434");

            Athlete paul = new Athlete { FirstName = "Paul", LastName = "McCartney", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = saveNow };
            ApplicationUser p = new ApplicationUser { Email = "paul@msn.com", UserName = "paul@msn.com", Athlete = paul };
            manager.Create(p, "123434");

            Athlete ringo = new Athlete { FirstName = "Ringo", LastName = "Starr", Address = "123 Main", City = "Cottleville", State = "MO", Zip = "63367", AthleteType = AthleteTypes.PersonalTraining, UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = saveNow };
            ApplicationUser r = new ApplicationUser { Email = "ringo@msn.com", UserName = "ringo@msn.com", Athlete = ringo };
            manager.Create(r, "123434");

            var s6 = new Session { Hour = 6, Day = saveNow, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining };
            var s7 = new Session { Hour = 7, Day = saveNow, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining };
            var s8 = new Session { Hour = 8, Day = saveNow, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining };
            var s9 = new Session { Hour = 9, Day = saveNow, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining };
            var s10 = new Session { Hour = 10, Day = saveNow, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining };
            //var s6Athlete1 = new SessionAthlete {Athlete = george, Session = s6, Confirmed = false};
            //var s6Athlete2 = new SessionAthlete {Athlete = paul, Session = s6, Confirmed = false};
            //var s7Athlete1 = new SessionAthlete {Athlete = john, Session = s7, Confirmed = false};
            //var s7Athlete2 = new SessionAthlete {Athlete = ringo, Session = s7, Confirmed = false};
            //var s8Athlete1 = new SessionAthlete {Athlete = ringo, Session = s8, Confirmed = false};
            //var s9Athlete1 = new SessionAthlete {Athlete = george, Session = s9, Confirmed = false};
            //var s10Athlete1 = new SessionAthlete {Athlete = george, Session = s10, Confirmed = false};

            var sessionAthletes = new List<SessionAthlete>
            {
                new SessionAthlete {Athlete = george, Session = s6, Confirmed = false},
                new SessionAthlete {Athlete = paul, Session = s6, Confirmed = false},
                new SessionAthlete {Athlete = john, Session = s7, Confirmed = false},
                new SessionAthlete {Athlete = ringo, Session = s8, Confirmed = false},
                new SessionAthlete {Athlete = ringo, Session = s8, Confirmed = false},
                new SessionAthlete {Athlete = george, Session = s9, Confirmed = false}

            };
            sessionAthletes.ForEach(s => context.SessionAthletes.AddOrUpdate(a => a.Athlete, s));

            //List<SessionAthlete> sixAmAthletes = new List<SessionAthlete>{s6Athlete1, s6Athlete2};
            //List<SessionAthlete> sevenAmAthletes = new List<SessionAthlete>{s7Athlete1, s7Athlete2};
            //List<SessionAthlete> eightAmAthletes = new List<SessionAthlete>{s8Athlete1};
            //List<SessionAthlete> nineAmAthletes = new List<SessionAthlete>{s9Athlete1};
            //List<SessionAthlete> tenAmAthletes = new List<SessionAthlete>{s10Athlete1};
            //List<SessionAthlete> empty = new List<SessionAthlete>();

            //var sessions = new List<Session>
            //{
            //    new Session{Hour =6, Day =saveNow, SessionAthletes =sixAmAthletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =7, Day =saveNow, SessionAthletes=sevenAmAthletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =8, Day =saveNow, SessionAthletes=eightAmAthletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =9, Day =saveNow, SessionAthletes=nineAmAthletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =10, Day =saveNow, SessionAthletes=tenAmAthletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =16, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =17, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{Hour =18, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //};
            //sessions.ForEach(s => context.Sessions.AddOrUpdate(a => a.Hour, s));
            //var sessionsSports = new List<Session>
            //{
            //    new Session{Hour =16, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =17, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =18, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =19, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{Hour =20, Day =saveNow, SessionAthletes=empty, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //};
            //sessionsSports.ForEach(s => context.Sessions.AddOrUpdate(a => a.Hour, s));

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

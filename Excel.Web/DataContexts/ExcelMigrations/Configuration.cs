namespace Excel.Web.DataContexts.ExcelMigrations
{
    using Excel.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Excel.Web.DataContexts.ExcelDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"DataContexts\ExcelMigrations";
        }

        protected override void Seed(Excel.Web.DataContexts.ExcelDb context)
        {
            var trainers = new List<Trainer>
            {
                new Trainer{FirstName="Kenny", LastName="Ball", Address="444 Primrose", City="Dardenne", State="MO", Zip="63368"}
            };
            trainers.ForEach(s => context.Trainer.AddOrUpdate(t => t.LastName, s));
            SaveChanges(context);

            var athletes = new List<Athlete>
            {
                new Athlete{FirstName="Joe", LastName="Jones", Address="123 Main", City="Cottleville", State="MO", Zip="63367"},
                new Athlete{FirstName="Mike", LastName="Moss", Address="123 Main", City="Cottleville", State="MO", Zip="63367"},
                new Athlete{FirstName="Adam", LastName="Ant", Address="123 Main", City="Cottleville", State="MO", Zip="63367"},
                new Athlete{FirstName="Mary", LastName="Shelly", Address="123 Main", City="Cottleville", State="MO", Zip="63367"},
            };
            athletes.ForEach(s => context.Athletes.AddOrUpdate(a => a.LastName, s));

            SaveChanges(context);
            
            var session6Athletes = new List<Athlete> { athletes[0], athletes[1] };
            var session7Athletes = new List<Athlete> { athletes[1], athletes[2] };
            var session8Athletes = new List<Athlete> { athletes[0], athletes[3] };
            var kenny = trainers[0];
            var sessions = new List<Session>
            {
                new Session{Hour =6, Day =DateTime.Parse("2015-06-06"), Athletes=session6Athletes, Trainer = kenny},
                new Session{Hour =7, Day =DateTime.Parse("2015-06-06"), Athletes=session7Athletes, Trainer = kenny},
                new Session{Hour =8, Day =DateTime.Parse("2015-06-06"), Athletes=session8Athletes, Trainer = kenny},
            };
            sessions.ForEach(s => context.Sessions.AddOrUpdate(a => a.Hour, s));
            SaveChanges(context);
            base.Seed(context);
           
        }

        private void SaveChanges(DbContext context)
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

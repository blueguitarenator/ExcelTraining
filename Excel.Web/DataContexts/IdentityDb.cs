using Excel.Entities;
using Excel.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Excel.Web.DataContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        virtual public DbSet<Athlete> Athletes { get; set; }
        virtual public DbSet<Session> Sessions { get; set; }
        virtual public DbSet<Location> Locations { get; set; }
        virtual public DbSet<Schedule> Schedules { get; set; }
        virtual public DbSet<SessionAthlete> SessionAthletes { get; set; }
        virtual public DbSet<Motd> Motd { get; set; }
        virtual public DbSet<HearAboutUs> HearAboutUs { get; set; }
        virtual public DbSet<InjuryNote> InjuryNotes { get; set; }
        virtual public DbSet<CellPhoneCarrier> CellPhoneCarriers { get; set; }


        public IdentityDb()
            : base("DefaultConnection")
        {
            //Database.SetInitializer<DbContext>(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("identity");
            modelBuilder.Entity<SessionAthlete>().HasKey(e => new {e.AthleteId, e.SessionId});
            base.OnModelCreating(modelBuilder);
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }

    }
}
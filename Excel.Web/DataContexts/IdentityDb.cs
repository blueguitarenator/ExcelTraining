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

        public IdentityDb()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("identity");
            base.OnModelCreating(modelBuilder);
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }
    }
}
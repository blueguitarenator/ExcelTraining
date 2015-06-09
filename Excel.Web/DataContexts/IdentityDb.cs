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
        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<Trainer> Trainer { get; set; }
        public DbSet<Session> Sessions { get; set; }

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
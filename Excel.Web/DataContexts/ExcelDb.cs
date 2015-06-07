using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Excel.Entities;
using System.Diagnostics;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;

namespace Excel.Web.DataContexts
{
    public class ExcelDb : DbContext
    {
        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<Trainer> Trainer { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public ExcelDb()
            : base("DefaultConnection")
        {
            Database.Log = sql => Debug.Write(sql);
            //Database.SetInitializer<ExcelDb>(new CreateDatabaseIfNotExists<ExcelDb>());

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("athletes");
            base.OnModelCreating(modelBuilder);
        }

    }
}
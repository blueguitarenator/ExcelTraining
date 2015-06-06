using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Excel.Entities;
using System.Diagnostics;

namespace Excel.Web.DataContexts
{
    public class ExcelDb : DbContext
    {
        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<Trainer> Trainer { get; set; }

        public ExcelDb()
            : base("DefaultConnection")
        {
            Database.Log = sql => Debug.Write(sql);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("athletes");
            base.OnModelCreating(modelBuilder);
        }

    }
}
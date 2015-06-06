﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    public class Athlete
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        [StringLength(255)]
        public string City { get; set; }
        [Required]
        [StringLength(255)]
        public string State { get; set; }
        [Required]
        [StringLength(255)]
        public string Zip { get; set; }
        [Required]
        public AthleteTypes AthleteType { get; set; }
    }
}
// update-database -ConfigurationTypeName Excel.Web.DataContexts.ExcelMigrations.Configuration
// add-migration -ConfigurationTypeName Excel.Web.DataContexts.ExcelMigrations.Configuration "DefaultSchema"
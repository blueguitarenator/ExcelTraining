﻿
using System;
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
        [StringLength(12)]
        public string CellNumber { get; set; }
        [Required]
        public AthleteTypes AthleteType { get; set; }
        [Required]
        public UserTypes UserType { get; set; }

        public virtual ICollection<SessionAthlete> SessionAthletes { get; set; }

        public virtual int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public virtual int? HearAboutUsId { get; set; }
        [Display(Name = "How you heard")]
        public virtual HearAboutUs HearAboutUs { get; set; }

        public virtual int? CellPhoneCarrierId { get; set; }
        [Display(Name = "Cell Phone Carrier")]
        public virtual CellPhoneCarrier CellPhoneCarrier { get; set; }

        public virtual ICollection<InjuryNote> InjuryNotes { get; set; }

        public DateTime EnrollmentDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Name")]
        public string FullName
        {
            get {  return FirstName + " " +  LastName ;}
        }
    }
}
// update-database -ConfigurationTypeName Excel.Web.DataContexts.ExcelMigrations.Configuration
// add-migration -ConfigurationTypeName Excel.Web.DataContexts.ExcelMigrations.Configuration "SessionData"
// Update-Database -ConfigurationTypeName Excel.Web.DataContexts.ExcelMigrations.Configuration -TargetMigration "201506062138571_AddAthleteType"
// get-migrations -ConfigurationTypeName Excel.Web.DataContexts.ExcelMigrations.Configuration

// scorched earth
// sqllocaldb.exe stop v11.0
// sqllocaldb.exe delete v11.0
// then update identity and excel

// add-migration -ConfigurationTypeName Excel.Web.DataContexts.IdentityMigrations.Configuration "AddAthleteToIdentity"


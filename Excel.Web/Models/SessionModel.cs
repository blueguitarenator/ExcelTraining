using Excel.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Excel.Web.Models
{
    public class SessionModel
    {
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name="Session Date:")]
        public DateTime SessionDateTime { get; set; }

        public SelectList LocationSelectList { get; set; }
        public int SelectedLocationId { get; set; }
        public AthleteTypes AthleteType{ get; set; }

        private SessionsWithAthletes allAthletes;
        public SessionsWithAthletes SessionsWithAthletes
        {
            get { return allAthletes; }
            set { allAthletes = value; }
        }

    }
}
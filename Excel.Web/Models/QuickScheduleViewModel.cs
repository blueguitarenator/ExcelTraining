using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class ConfirmedAthlete
    {
        public Athlete Athlete { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class QuickScheduleViewModel
    {
        private List<ConfirmedAthlete> quickAthletes;
        public string SessionDate { get; set; }
        public string SessionTime { get; set; }
        public int SessionId { get; set; }

        public List<ConfirmedAthlete> QuickAthletes
        {
            get { return quickAthletes; }
            set { quickAthletes = value; }
        }
    }
}
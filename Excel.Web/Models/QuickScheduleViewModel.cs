using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{

    public class QuickScheduleViewModel
    {
        private List<Athlete> quickAthletes;
        public string SessionDate { get; set; }
        public string SessionTime { get; set; }

        public List<Athlete> QuickAthletes
        {
            get { return quickAthletes; }
            set { quickAthletes = value; }
        }
    }
}
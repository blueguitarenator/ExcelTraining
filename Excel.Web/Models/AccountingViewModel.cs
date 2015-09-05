using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel.Entities;

namespace Excel.Web.Models
{
    public class DaySession
    {
        public string Date { get; set; }
        public List<SingleSession> DaySessionList { get; set; }
    }

    public class SingleSession
    {
        public int Hour { get; set; }
        public List<Athlete> Athletes { get; set; } 
    }

    public class AccountingViewModel
    {
        public List<DaySession> PersonalTrainingSessionsWithAthletes { get; set; } 
    }
}
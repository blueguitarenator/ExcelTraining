using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel.Entities;

namespace Excel.Web.Models
{
    public class SingleSession
    {
        public int Hour { get; set; }
        public string Date { get; set; }
        public List<Athlete> Athletes { get; set; } 
    }

    public class AccountingViewModel
    {
        public List<SingleSession> PersonalTrainingSessionsWithAthletes { get; set; } 
    }
}
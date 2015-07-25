using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class AthleteData
    {
        private List<String> athletes;
        public int Hour { get; set; }
        public string Time { get; set; }
        public string DivId { get; set; }
        public List<string> Athletes
        {
            get { return athletes; }
            set { athletes = value; }
        }
    }

    public class SessionsWithAthletes
    {
        private List<AthleteData> athleteData;


        public List<AthleteData> AthleteData
        {
            get { return athleteData; }
            set { athleteData = value; }
        }

    }
}
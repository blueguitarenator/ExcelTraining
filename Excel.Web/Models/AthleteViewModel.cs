using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class AthleteViewModel
    {
        private List<Athlete> athletes;
        private List<Location> locations;

        public List<Athlete> Athletes
        {
            get { return athletes; }
            set { athletes = value; }
        }

        public List<Location> Locations
        {
            get { return locations; }
            set { locations = value; }
        }


    }
}
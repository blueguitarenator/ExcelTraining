using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class ScheduleViewModel
    {
        private List<Schedule> dardennePersonalTrainingSchedule;
        private List<Schedule> dardenneSportsTrainingSchedule;

        public List<Schedule> DardennePersonalTrainingSchedule
        {
            get { return dardennePersonalTrainingSchedule; }
            set { dardennePersonalTrainingSchedule = value; }
        }

        public List<Schedule> DardenneSportsTrainingSchedule
        {
            get { return dardenneSportsTrainingSchedule; }
            set { dardenneSportsTrainingSchedule = value; }
        }
    }
}
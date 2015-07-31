using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class ScheduleViewModel
    {
        private IEnumerable<Schedule> dardennePersonalTrainingSchedule;
        private IEnumerable<Schedule> dardenneSportsTrainingSchedule;

        public IEnumerable<Schedule> DardennePersonalTrainingSchedule
        {
            get { return dardennePersonalTrainingSchedule; }
            set { dardennePersonalTrainingSchedule = value; }
        }

        public IEnumerable<Schedule> DardenneSportsTrainingSchedule
        {
            get { return dardenneSportsTrainingSchedule; }
            set { dardenneSportsTrainingSchedule = value; }
        }
    }
}
using Excel.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class SessionModel
    {
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}",
               ApplyFormatInEditMode = true)]
        [Display(Name="Session Date:")]
        public DateTime SessionDateTime { get; set; }

        public int Hour { get; set; }

        private List<Athlete> sixAmPersonalTraining;
        private List<Athlete> sevenAmPersonalTraining;

        public List<Athlete> SixAmPersonalTraining
        {
            get { return sixAmPersonalTraining; }
            set { sixAmPersonalTraining = value; }
        }

        public List<Athlete> SevenAmPersonalTraining
        {
            get { return sevenAmPersonalTraining; }
            set { sevenAmPersonalTraining = value; }
        }

    }
}
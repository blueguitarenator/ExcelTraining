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

        public int Hour { get; set; }
        public Location Location { get; set; }
        public SelectList LocationSelectList { get; set; }
        public int SelectedLocationId { get; set; }

        private List<Athlete> sixAmPersonalTraining;
        private List<Athlete> sevenAmPersonalTraining;
        private List<Athlete> eightAmPersonalTraining;
        private List<Athlete> nineAmPersonalTraining;
        private List<Athlete> tenAmPersonalTraining;
        private List<Athlete> fourPmPersonalTraining;
        private List<Athlete> fivePmPersonalTraining;
        private List<Athlete> sixPmPersonalTraining;

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

        public List<Athlete> EightAmPersonalTraining
        {
            get { return eightAmPersonalTraining; }
            set { eightAmPersonalTraining = value; }
        }

        public List<Athlete> NineAmPersonalTraining
        {
            get { return nineAmPersonalTraining; }
            set { nineAmPersonalTraining = value; }
        }

        public List<Athlete> TenAmPersonalTraining
        {
            get { return tenAmPersonalTraining; }
            set { tenAmPersonalTraining = value; }
        }

        public List<Athlete> FourPmPersonalTraining
        {
            get { return fourPmPersonalTraining; }
            set { fourPmPersonalTraining = value; }
        }

        public List<Athlete> FivePmPersonalTraining
        {
            get { return fivePmPersonalTraining; }
            set { fivePmPersonalTraining = value; }
        }

        public List<Athlete> SixPmPersonalTraining
        {
            get { return sixPmPersonalTraining; }
            set { sixPmPersonalTraining = value; }
        }

    }
}
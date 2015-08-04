using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Excel.Web.Models
{
    public class TrainerQueueViewModel
    {
        public string SessionDate { get; set; }
        public string SessionTime { get; set; }

        public int PersonalTrainingSessionId { get; set; }
        public int SportsTrainingSessionId { get; set; }

        public SelectList PersonalTrainerSelectList { get; set; }
        public int PersonalTrainerId { get; set; }
        public SelectList SportsTrainerSelectList { get; set; }
        public Athlete SportsTrainerId { get; set; }

        private List<Athlete> personalAthletes;
        private List<Athlete> sportsAthletes;
        private IEnumerable<Athlete> trainerList;

        public IEnumerable<Athlete> TrainerList
        {
            get { return trainerList; }
            set { trainerList = value; }
        }

        public List<Athlete> PersonalAthletes
        {
            get { return personalAthletes; }
            set { personalAthletes = value; }
        }

        public List<Athlete> SportsAthletes
        {
            get { return sportsAthletes; }
            set { sportsAthletes = value; }
        }
    }
}
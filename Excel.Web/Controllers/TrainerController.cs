using Excel.Entities;
using Excel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Excel.Web.Controllers
{
    public class TrainerController : Controller
    {
        private IAthleteRepository athleteRepository;
        private QuickHelper helper = new QuickHelper();
      
        public TrainerController()
        {
            athleteRepository = new AthleteRepository();
            //GetUserId = () => User.Identity.GetUserId();
        }

        public TrainerController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Trainer
        public ActionResult Index()
        {
            TrainerQueueViewModel model = new TrainerQueueViewModel();

            DateTime nextSession = helper.GetNextSession();
            model.SessionDate = nextSession.ToLongDateString();
            model.SessionTime = helper.GetSessionTimeString(nextSession);
            int locationId = helper.GetDardenne(athleteRepository).Id;
            Excel.Entities.Session personalTrainingSession = helper.GetOrCreateSession(nextSession.Hour, nextSession, locationId, Entities.AthleteTypes.PersonalTraining, athleteRepository);
            model.PersonalTrainingSessionId = 0;
            model.SportsTrainerId = 0;
            if (personalTrainingSession != null)
            {
                model.PersonalTrainerId = GetPersonalTrainerId(personalTrainingSession.Id);
                model.PersonalTrainingSessionId = personalTrainingSession.Id;
                model.PersonalAthletes = athleteRepository.GetPersonalTrainingAthletes(personalTrainingSession.Id, locationId).ToList();
            }
            else
            {
                model.PersonalAthletes = new List<Athlete>();
            }
            Excel.Entities.Session sportsTrainingSession = helper.GetOrCreateSession(nextSession.Hour, nextSession, locationId, Entities.AthleteTypes.SportsTraining, athleteRepository);
            if (sportsTrainingSession != null)
            {
                model.SportsTrainerId = GetPersonalTrainerId(sportsTrainingSession.Id);
                model.SportsTrainingSessionId = sportsTrainingSession.Id;
                model.SportsAthletes = athleteRepository.GetSportsTrainingAthletes(sportsTrainingSession.Id, locationId).ToList();
            }
            else
            {
                model.SportsAthletes = new List<Athlete>();
            }

            model.SportsTrainerId = 0;
            LoadPersonalTrainerSelectList(model);
            LoadSportsTrainerSelectList(model);
            return View(model);
        }

        private int GetPersonalTrainerId(int sessionId)
        {
            var trainer = athleteRepository.GetSesssionTrainer(sessionId);
            if (trainer != null)
            {
                return trainer.Id;
            }
            return 0;
        }

        public ActionResult ChangeTrainer(int trainerId, int session)
        {
            var trainer = athleteRepository.GetSesssionTrainer(session);
            if (trainer != null)
            {
                athleteRepository.RemoveAthleteFromSession(session, trainer.Id);
            }
            athleteRepository.AddAthleteToSession(session, trainerId);

            return RedirectToAction("Index");
        }
        
        private void LoadPersonalTrainerSelectList(TrainerQueueViewModel model)
        {
            Athlete notSet = new Athlete { Id = 0, FirstName = "not", LastName = "set" };
            List<Athlete> allTrainers = new List<Athlete> { notSet };
            IEnumerable<Athlete> allPlusNotSet = allTrainers.Concat(athleteRepository.GetAllTrainers().ToList());
            model.PersonalTrainerSelectList = new SelectList(allPlusNotSet, "Id", "FullName", model.PersonalTrainerId);
        }

        private void LoadSportsTrainerSelectList(TrainerQueueViewModel model)
        {
            Athlete notSet = new Athlete { Id = 0, FirstName = "not", LastName = "set" };
            List<Athlete> allTrainers = new List<Athlete> { notSet };
            IEnumerable<Athlete> allPlusNotSet = allTrainers.Concat(athleteRepository.GetAllTrainers().ToList());
            model.SportsTrainerSelectList = new SelectList(allPlusNotSet, "Id", "FullName", model.SportsTrainerId);
        }
    }
}
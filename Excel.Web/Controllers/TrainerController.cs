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
            if (personalTrainingSession != null)
            {
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
                model.SportsTrainingSessionId = sportsTrainingSession.Id;
                model.SportsAthletes = athleteRepository.GetSportsTrainingAthletes(sportsTrainingSession.Id, locationId).ToList();
            }
            else
            {
                model.SportsAthletes = new List<Athlete>();
            }
            // TODO:
            // set view model selected trainer - personal and sport
            LoadPersonalTrainerSelectList(model);
            LoadSportsTrainerSelectList(model);
            return View(model);
        }

        public ActionResult ChangePersonalTrainer(int trainerId, int session)
        {
            var trainer = athleteRepository.GetSesssionTrainer(session);
            // TODO:
            // get session
            // remove any trainer
            // add this trainer
            // save changes
            return RedirectToAction("Index");
        }
        public ActionResult ChangeSportsTrainer(int trainerId, int session)
        {
            var trainer = athleteRepository.GetSesssionTrainer(session);
            // TODO:
            // get session
            // remove any trainer
            // add this trainer
            // save changes
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
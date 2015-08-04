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
            model.PersonalAthletes = athleteRepository.GetPersonalTrainingAthletes(personalTrainingSession.Id, locationId).ToList();
            Excel.Entities.Session sportsTrainingSession = helper.GetOrCreateSession(nextSession.Hour, nextSession, locationId, Entities.AthleteTypes.SportsTraining, athleteRepository);
            model.SportsAthletes = athleteRepository.GetSportsTrainingAthletes(sportsTrainingSession.Id, locationId).ToList();
            // TODO:
            // set view model selected trainer - personal and sport
            LoadPersonalTrainerSelectList(model);
            LoadSportsTrainerSelectList(model);
            return View(model);
        }

        public ActionResult ChangePersonalTrainer(int trainerId)
        {
            // TODO:
            // get session
            // remove any trainer
            // add this trainer
            // save changes
            return RedirectToAction("Index");
        }
        public ActionResult ChangeSportsTrainer(int trainerId)
        {
            // TODO:
            // get session
            // remove any trainer
            // add this trainer
            // save changes
            return RedirectToAction("Index");
        }
        
        private void LoadPersonalTrainerSelectList(TrainerQueueViewModel model)
        {
            model.PersonalTrainerSelectList = new SelectList(athleteRepository.GetAllTrainers(), "Id", "FullName", model.PersonalTrainerId);
        }

        private void LoadSportsTrainerSelectList(TrainerQueueViewModel model)
        {
            model.SportsTrainerSelectList = new SelectList(athleteRepository.GetAllTrainers(), "Id", "FullName", model.SportsTrainerId);
        }
    }
}
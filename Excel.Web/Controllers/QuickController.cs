using Excel.Entities;
using Excel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Excel.Web.Controllers
{
    public class QuickController : Controller
    {
        private IAthleteRepository athleteRepository;
        private QuickHelper helper = new QuickHelper();
      
        public QuickController()
        {
            athleteRepository = new AthleteRepository();
            //GetUserId = () => User.Identity.GetUserId();
        }

        public QuickController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Quick
        public ActionResult Index()
        {
            QuickScheduleViewModel quickScheduleViewModel = new QuickScheduleViewModel();

            DateTime nextSession = helper.GetNextSession();
            quickScheduleViewModel.SessionDate = nextSession.ToLongDateString();
            quickScheduleViewModel.SessionTime = helper.GetSessionTimeString(nextSession);
            int locationId = helper.GetDardenne(athleteRepository).Id;
            Session session = helper.GetOrCreateSession(nextSession.Hour, nextSession, locationId, athleteRepository);
            quickScheduleViewModel.QuickAthletes = athleteRepository.GetPersonalTrainingAthletes(session.Id, locationId).ToList();
            quickScheduleViewModel.QuickAthletes = quickScheduleViewModel.QuickAthletes.Concat(athleteRepository.GetSportsTrainingAthletes(session.Id, locationId)).ToList();
            return View(quickScheduleViewModel);
        }

        public ActionResult Signup(string email)
        {
            var saveNow = helper.GetNextSession();
            var session = helper.GetOrCreateSession(saveNow.Hour, saveNow, helper.GetDardenne(athleteRepository).Id, athleteRepository);
            var athlete = athleteRepository.GetAthleteByEmail(email);
            if (athlete != null)
            {
                athleteRepository.AddAthleteToSession(session.Id, athlete.Id);
            }
            return RedirectToAction("Index"); 
        }        
    }
}
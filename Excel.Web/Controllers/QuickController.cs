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

            DateTime nextSession = helper.GetNextSession(athleteRepository);
            quickScheduleViewModel.SessionDate = nextSession.ToLongDateString();
            quickScheduleViewModel.SessionTime = helper.GetSessionTimeString(nextSession);
            int locationId = helper.GetDardenne(athleteRepository).Id;

            Session session = helper.GetOrCreateSession(nextSession.Hour, nextSession, locationId, AthleteTypes.PersonalTraining, athleteRepository);
            quickScheduleViewModel.SessionId = session.Id;
            IEnumerable<Athlete> athletesPersonal = athleteRepository.GetPersonalTrainingAthletes(session.Id, locationId).ToList();
            IEnumerable<Athlete> allAthletes = athletesPersonal.Concat(athleteRepository.GetSportsTrainingAthletes(session.Id, locationId)).ToList();

            List<ConfirmedAthlete> confirmedAthletes = new List<ConfirmedAthlete>();
            foreach (var athlete in allAthletes)
            {
                bool status = athleteRepository.IsAthleteConfirmed(session.Id, athlete.Id);
                ConfirmedAthlete confirmedAthlete = new ConfirmedAthlete {Athlete = athlete, IsConfirmed = status};
                confirmedAthletes.Add(confirmedAthlete);
            }
            quickScheduleViewModel.QuickAthletes = confirmedAthletes;
            return View(quickScheduleViewModel);
        }

        public ActionResult Signup(string email)
        {
            var athlete = athleteRepository.GetAthleteByEmail(email);
            if (athlete != null)
            {
                var saveNow = helper.GetNextSession(athleteRepository);
                var session = helper.GetOrCreateSession(saveNow.Hour, saveNow, helper.GetDardenne(athleteRepository).Id,
                    athlete.AthleteType, athleteRepository);
                athleteRepository.AddAthleteToSession(session.Id, athlete.Id);
                athleteRepository.ConfirmAthlete(session.Id, athlete.Id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Confirm(int? athleteId, int? sessionId)
        {
            if (athleteId.HasValue && sessionId.HasValue)
            athleteRepository.ConfirmAthlete(sessionId.Value, athleteId.Value);
            return RedirectToAction("Index");
        }
    }
}
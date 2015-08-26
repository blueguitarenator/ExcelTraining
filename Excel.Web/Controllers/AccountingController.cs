using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel.Entities;
using Excel.Web.Models;

namespace Excel.Web.Controllers
{
    public class AccountingController : Controller
    {
        private IAthleteRepository athleteRepository;
        private QuickHelper helper = new QuickHelper();

        public AccountingController()
        {
            athleteRepository = new AthleteRepository();
            //GetUserId = () => User.Identity.GetUserId();
        }

        public AccountingController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Accounting
        public ActionResult Index()
        { 
            List<SingleSession> allPersonalTrainingSessionAthletes = new List<SingleSession>();
            AccountingViewModel model = new AccountingViewModel();
            List<Session> sessions = athleteRepository.GetAllSessions().ToList();
            foreach (var session in sessions)
            {
                SingleSession singleSession = new SingleSession();
                singleSession.Athletes = athleteRepository.GetPersonalTrainingAthletes(session.Id, 1).ToList();
                singleSession.Hour = session.Hour;
                singleSession.Date = session.Day.ToLongDateString();
                allPersonalTrainingSessionAthletes.Add(singleSession);
            }
            model.PersonalTrainingSessionsWithAthletes = allPersonalTrainingSessionAthletes;
            return View(model);
        }
    }
}
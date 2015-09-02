using Excel.Entities;
using Excel.Web.DataContexts;
using Excel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Excel.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IAthleteRepository athleteRepository;

        public Func<string> GetUserId; //For testing

        public DashboardController()
        {
            athleteRepository = new AthleteRepository();
            GetUserId = () => User.Identity.GetUserId();
        }

        public DashboardController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            if (CheckForQuickSchedule())
            {
                return RedirectToAction("Index", "Quick");
            }
            DashboardModel model = new DashboardModel();

            model.MySessions = GetFutureSessions();
            model.TotalSession = GetPastSessionCount();

            return View(model);
        }

        private bool CheckForQuickSchedule()
        {
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);
            if (athlete.FirstName == "Location" && athlete.LastName == "Dardenne")
            {
                return true;
            }
            return false;
        }

        public ActionResult RemoveFromSession(int sessionId)
        {
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);

            var session = athleteRepository.GetSessionById(sessionId);
            athleteRepository.RemoveAthleteFromSession(session.Id, athlete.Id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                athleteRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Session> GetFutureSessions()
        {
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);
            return athleteRepository.GetFutureSessions(athlete.Id).ToList();
        }

        private int GetPastSessionCount()
        {
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);
            return athleteRepository.GetPastSessions(athlete.Id).Count();
        }

    }
}
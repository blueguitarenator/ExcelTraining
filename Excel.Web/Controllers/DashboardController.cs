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
            DashboardModel model = new DashboardModel();

            model.MySessions = getFutureSessions();
            model.TotalSession = getPastSessionCount();

            return View(model);
        }

        public ActionResult RemoveFromSession(int sessionId)
        {
            DashboardModel model = new DashboardModel();
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);

            var session = athleteRepository.GetSessionById(sessionId);
            athleteRepository.RemoveAthleteFromSession(session.Id, athlete.Id);
            athleteRepository.SaveChanges();

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

        private List<Session> getFutureSessions()
        {
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);
            return athleteRepository.GetFutureSessions(athlete.Id).ToList();
        }

        private int getPastSessionCount()
        {
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);
            return athleteRepository.GetPastSessions(athlete.Id).Count();
        }

    }
}
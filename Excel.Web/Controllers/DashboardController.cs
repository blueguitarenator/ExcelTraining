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

        public DashboardController(IAthleteRepository db)
        {
            GetUserId = () => User.Identity.GetUserId();
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

        public ActionResult RemoveSession(int sessionId)
        {
            DashboardModel model = new DashboardModel();
            var userId = GetUserId();
            var athlete = athleteRepository.GetAthleteByUserId(userId);

            var session = athleteRepository.GetSessionById(sessionId);
            athleteRepository.SaveChanges();

            return RedirectToAction("Index");
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
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

            //DateTime saveNow = DateTime.Now.Date;
            DateTime saveNow = new DateTime(2015, 7, 27);

            quickScheduleViewModel.QuickAthletes = getSessionListNames(6 /*saveNow.Hour*/, saveNow, getDardenne().Id);

            return View(quickScheduleViewModel);
        }

        private Location getDardenne()
        {
            IEnumerable<Location> locationList = athleteRepository.GetLocations();
            Location loc = locationList.ElementAt(0);
            if (loc.Name.Contains("Dardenne"))
            {
                return loc;
            }
            loc = locationList.ElementAt(1);
            if (loc.Name.Contains("Dardenne"))
            {
                return loc;
            }
            return null;        
        }

        private List<Athlete> getSessionListNames(int hour, DateTime dt, int locationId)
        {
            Session session = getOrCreateSession(hour, dt, locationId);
            List<Athlete> athletes = new List<Athlete>();
            athletes = session.Athletes.ToList();
            //if (session != null)
            //{
            //    athletes.Concat(athleteRepository.GetPersonalTrainingAthletes(session.Id, locationId));
            //    athletes.Concat(athleteRepository.GetSportsTrainingAthletes(session.Id, locationId));
            //}
            return athletes;
        }

        private Session getOrCreateSession(int hour, DateTime dt, int locationId)
        {
            Session session = athleteRepository.GetSession(hour, dt, locationId);
            if (session == null)
            {
                athleteRepository.Write_CreateSessions(dt, locationId);
                session = athleteRepository.GetSession(hour, dt, locationId);
            }
            return session;
        }
    }
}
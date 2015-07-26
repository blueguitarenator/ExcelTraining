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

            DateTime nextSession = getNextSession();
            quickScheduleViewModel.SessionDate = nextSession.ToLongDateString();
            setSessionTime(quickScheduleViewModel, nextSession);

            quickScheduleViewModel.QuickAthletes = getSessionListNames(nextSession.Hour, nextSession, getDardenne().Id);

            return View(quickScheduleViewModel);
        }

        private void setSessionTime(QuickScheduleViewModel model, DateTime nextSession)
        {
            model.SessionTime = nextSession.Hour.ToString() + ":00";
            if (nextSession.Hour < 12)
            {
                model.SessionTime = nextSession.Hour.ToString() + ":00 AM";
            }
            else
            {
                int hour = nextSession.Hour - 12;
                model.SessionTime = hour.ToString() + ":00 PM";
            }
        }

        private DateTime getNextSession()
        {
            DateTime nextSession = DateTime.Now;
            bool isFuture = false;
            while (!isSessionTime(nextSession))
            {
                isFuture = true;
                nextSession = nextSession.AddHours(1);
            }
            if (!isFuture && nextSession.Minute > 15)
            {
                nextSession = nextSession.AddHours(1);
            }
            return nextSession;
        }

        private bool isSessionTime(DateTime nextSession)
        {
            return nextSession.DayOfWeek != DayOfWeek.Saturday &&
                nextSession.DayOfWeek != DayOfWeek.Sunday &&
                (nextSession.Hour > 5 && nextSession.Hour < 11 ||
                nextSession.Hour > 15 && nextSession.Hour < 19);
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
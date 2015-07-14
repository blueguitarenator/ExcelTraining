using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Excel.Entities;
using Excel.Web.DataContexts;
using Excel.Web.Models;
using Microsoft.AspNet.Identity;

namespace Excel.Web.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private IAthleteRepository athleteRepository;

        public Func<string> GetUserId; //For testing

        public SessionController()
        {
            athleteRepository = new AthleteRepository();
            GetUserId = () => User.Identity.GetUserId();
        }

        public SessionController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Sessions
        public ActionResult Index()
        {
            SessionModel sessionModel = new SessionModel();

            var athlete = getThisAthlete();
            sessionModel.SessionDateTime = athlete.SelectedDate;
            sessionModel.SelectedLocationId = athlete.SelectedLocationId;

            LoadGrid(sessionModel);
            loadLocationSelectList(sessionModel);
            return View(sessionModel);
        }

        private void loadLocationSelectList(SessionModel sessionModel)
        {
            sessionModel.LocationSelectList = new SelectList(athleteRepository.GetLocations(), "Id", "Name", sessionModel.SelectedLocationId);
        }

        public ActionResult ChangeDate(DateTime dt)
        {
            var athlete = getThisAthlete();
            athlete.SelectedDate = dt;
            athleteRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeLocation(int locId)
        {
            var athlete = getThisAthlete();
            athlete.SelectedLocationId = locId;
            athleteRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        private void LoadGrid(SessionModel model)
        {
            model.SixAmPersonalTraining = getSessionList(6, model.SessionDateTime, model.SelectedLocationId);
            model.SevenAmPersonalTraining = getSessionList(7, model.SessionDateTime, model.SelectedLocationId);
            model.EightAmPersonalTraining = getSessionList(8, model.SessionDateTime, model.SelectedLocationId);
            model.NineAmPersonalTraining = getSessionList(9, model.SessionDateTime, model.SelectedLocationId);
            model.TenAmPersonalTraining = getSessionList(10, model.SessionDateTime, model.SelectedLocationId);
            model.FourPmPersonalTraining = getSessionList(16, model.SessionDateTime, model.SelectedLocationId);
            model.FivePmPersonalTraining = getSessionList(17, model.SessionDateTime, model.SelectedLocationId);
            model.SixPmPersonalTraining = getSessionList(18, model.SessionDateTime, model.SelectedLocationId);
        }

        private List<Athlete> getSessionList(int hour, DateTime dt, int locationId)
        {
            Session session = getOrCreateSession(hour, dt, locationId);
            
            IEnumerable<Athlete> data = null;
            if (session.Athletes != null)
            {
                data = athleteRepository.GetPersonalTrainingAthletes(session.Id);
            }
            int cnt = 0;
            if (data != null)
            {
                cnt = data.Count();
            }
            List<Athlete> openSlots = new List<Athlete>();
            for (int i = cnt; i < 16; i++)
            {
                openSlots.Add(new Athlete { LastName = "open" });
            }
            if (data != null)
            {
                data = data.Concat(openSlots);
                return data.ToList();
            }
            return openSlots;
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

        private Athlete getThisAthlete()
        {
            var userId = GetUserId();
            return athleteRepository.GetAthleteByUserId(userId);
        }

        // GET: Sessions/Add/5
        public PartialViewResult _OneSession(DateTime dt, int hour, int SelectedLocationId)
        {
            Session session = athleteRepository.GetSession(hour, dt, SelectedLocationId);
            athleteRepository.AddAthleteToSession(session.Id, getThisAthlete().Id);

            IEnumerable<Athlete> athletes = session.Athletes.Where(a => a.UserType != UserTypes.Trainer);
            List<Athlete> openSlots = new List<Athlete>();

            athletes = athletes.Concat(openSlots);
            int cnt = athletes.Count();
            for (int i = cnt; i < 16; i++)
            {
                openSlots.Add(new Athlete { LastName = "open" });
            }
            return PartialView(athletes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                athleteRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

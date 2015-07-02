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
        private IdentityDb db = new IdentityDb();

        // GET: Sessions
        public ActionResult Index()
        {
            SessionModel sessionModel = new SessionModel();

            var athlete = getThisAthlete();
            sessionModel.SessionDateTime = athlete.SelectedDate;
            sessionModel.SelectedLocationId = athlete.SelectedLocationId;

            sessionModel.Hour = 6;
            LoadGrid(sessionModel);
            loadLocationSelectList(sessionModel);
            return View(sessionModel);
        }

        private void loadLocationSelectList(SessionModel sessionModel)
        {
            sessionModel.LocationSelectList = new SelectList(db.Locations, "Id", "Name", sessionModel.SelectedLocationId);
        }

        [HttpPost]
        public ActionResult Index(SessionModel sessionModel)
        {
            var athlete = getThisAthlete();
            athlete.SelectedDate = sessionModel.SessionDateTime;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeDate(DateTime dt)
        {
            var athlete = getThisAthlete();
            athlete.SelectedDate = dt;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeLocation(int locId)
        {
            var athlete = getThisAthlete();
            athlete.SelectedLocationId = locId;
            db.SaveChanges();
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
            if (session == null)
            {
                session = getOrCreateSession(hour, dt, locationId);
            }
            IEnumerable<Athlete> data = null;
            if (session.Athletes != null)
            {
                data = session.Athletes.Where(a => a.UserType != UserTypes.Trainer);
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
            Session session = db.Sessions.Where(
                s => s.Hour == hour &&
                s.Day.Year == dt.Year &&
                s.Day.Month == dt.Month &&
                s.Day.Day == dt.Day &&
                s.LocationId == locationId).SingleOrDefault();
            if (session == null)
            {
                db.Sessions.Add(new Session { Day = dt, Hour = 6, LocationId = locationId });
                db.Sessions.Add(new Session { Day = dt, Hour = 7, LocationId = locationId });
                db.Sessions.Add(new Session { Day = dt, Hour = 8, LocationId = locationId });
                db.Sessions.Add(new Session { Day = dt, Hour = 9, LocationId = locationId });
                db.Sessions.Add(new Session { Day = dt, Hour = 10, LocationId = locationId });
                db.Sessions.Add(new Session { Day = dt, Hour = 16, LocationId = locationId });
                db.Sessions.Add(new Session { Day = dt, Hour = 17, LocationId = locationId });
                db.Sessions.Add(new Session { Day = dt, Hour = 18, LocationId = locationId });
                db.SaveChanges();
            }
            return session;
        }

        private Athlete getThisAthlete()
        {
            var userId = User.Identity.GetUserId();
            var appUser = db.Users.Where(u => u.Id == userId).SingleOrDefault();
            var athlete = db.Athletes.Where(a => a.Id == appUser.Athlete.Id).SingleOrDefault();
            return athlete;
        }

        // GET: Sessions/Add/5
        public PartialViewResult _OneSession(DateTime dt, int hour, int SelectedLocationId)
        {
            var session = getOrCreateSession(hour, dt, SelectedLocationId);
            session.Athletes.Add(getThisAthlete());
            db.SaveChanges();

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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

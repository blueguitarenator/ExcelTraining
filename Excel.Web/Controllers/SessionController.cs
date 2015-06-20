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
    public class SessionController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Sessions
        public ActionResult Index(DateTime? dt)
        {
            if (dt == null)
            {
                DateTime nowDateOnly = DateTime.Now.Date;
                dt = nowDateOnly;
            }

            DateTime saveNow = new DateTime(2015, 06, 06, 0, 0, 0);// DateTime.Now;
            ViewBag.TodaysDate = saveNow.ToShortDateString();

            SessionModel sessionModel = new SessionModel();
            sessionModel.SessionDateTime = saveNow;// (DateTime)dt;
            sessionModel.Hour = 6;
            return View(sessionModel);
        }

        public PartialViewResult _PersonalTrainingGrid(SessionModel model, bool? doAdd)
        {
            if (doAdd.HasValue && doAdd.Value)
            {

                var userId = User.Identity.GetUserId();
                var appUser = db.Users.Where(u => u.Id == userId).SingleOrDefault();
                var session = getOrCreateSession(model.Hour, model.SessionDateTime);
                var athlete = db.Athletes.Where(a => a.Id == appUser.Athlete.Id).SingleOrDefault();
                session.Athletes.Add(athlete);
                db.SaveChanges();
            }
            model.SixAmPersonalTraining = getSessionList(6, model.SessionDateTime);
            model.SevenAmPersonalTraining = getSessionList(7, model.SessionDateTime);
            return PartialView(model);

        }

        private List<Athlete> getSessionList(int hour, DateTime dt)
        {
            Session session = getOrCreateSession(hour, dt);

            IEnumerable<Athlete> data = session.Athletes;
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

        private Session getOrCreateSession(int hour, DateTime dt)
        {
            Session session = db.Sessions.Where(
                s => s.Hour == hour &&
                s.Day.Year == dt.Year &&
                s.Day.Month == dt.Month &&
                s.Day.Day == dt.Day).SingleOrDefault();
            if (session == null)
            {
                session = new Session { Day = dt, Hour = hour };
                db.Sessions.Add(session);
                db.SaveChanges();
            }
            return session;
        }

        // GET: Sessions/Add/5
        public PartialViewResult _OneSession(DateTime dt, int hour)
        {
            var userId = User.Identity.GetUserId();
            var appUser = db.Users.Where(u => u.Id == userId).SingleOrDefault();
            var session = getOrCreateSession(hour, dt);
            var athlete = db.Athletes.Where(a => a.Id == appUser.Athlete.Id).SingleOrDefault();
            session.Athletes.Add(athlete);
            db.SaveChanges();

            IEnumerable<Athlete> athletes = session.Athletes;
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

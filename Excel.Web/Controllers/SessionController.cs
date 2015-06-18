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
            int year = saveNow.Year;
            int month = saveNow.Month;
            int day = saveNow.Day;

            //List<SessionTableAthletes> sessionTableAthletes = new List<SessionTableAthletes>();
            //sessionTableAthletes.Add(getSessionTableForHour(6, saveNow));
            //sessionTableAthletes.Add(getSessionTableForHour(7, saveNow));
            //sessionTableAthletes.Add(getSessionTableForHour(8, saveNow));
            //sessionTableAthletes.Add(getSessionTableForHour(9, saveNow));

            SessionModel sessionModel = new SessionModel();
            sessionModel.SessionDateTime = saveNow;// (DateTime)dt;
            sessionModel.Hour = 6;
            
            
            return View(sessionModel);
        }


        //public ActionResult Index(SessionModel model)
        //{
        //    if (sessionModel == null)
        //    {
        //        sessionModel = new SessionModel();
        //        sessionModel.SessionDateTime = model.SessionDateTime;
        //    }
        //    return RedirectToAction("Index");
        //}

        public PartialViewResult GetSessionAthleteData(SessionModel model)
        {
            Session session = getOrCreateSession(model.Hour, model.SessionDateTime);
            //Session session = db.Sessions.Include(c => c.Athletes).Where(
            //    s => s.Hour == model.Hour &&
            //    s.Day.Year == model.SessionDateTime.Year &&
            //    s.Day.Month == model.SessionDateTime.Month &&
            //    s.Day.Day == model.SessionDateTime.Day).SingleOrDefault();

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
                return PartialView(data);
            }
            return PartialView(openSlots);
        }

        private Session getOrCreateSession(int hour, DateTime dt)
        {
            hour = 6;
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

        private SessionTableAthletes getSessionTableForHour(int hour, DateTime dt)
        {
            SessionTableAthletes sta = new SessionTableAthletes();
            sta.SessionAthletes = new string[16];
            sta.hour = hour;
            sta.SessionDate = DateTime.Now;
            
            Session session = db.Sessions.Include(c => c.Athletes).Where(
                s => s.Hour == hour &&
                s.Day.Year == dt.Year &&
                s.Day.Month == dt.Month && 
                s.Day.Day == dt.Day).SingleOrDefault();

            for (int i = 0; i < 16; i++)
            {
                sta.SessionAthletes[i] = "open";
            }
            if (session != null)
            {
                Athlete[] athletesAndTrainer = session.Athletes.ToArray();
                Athlete[] athletesOnly = athletesAndTrainer.Where(a => a.UserType == UserTypes.Athlete).ToArray();
                
                var counter = 0;
                for (var i = 0; i < 16; i++)
                {
                    if (counter < athletesOnly.Count())
                    {
                        string name = athletesOnly[counter].FirstName + " " + athletesOnly[counter].LastName;
                        sta.SessionAthletes[i] = name;
                    }
                    counter++;
                }
            }
            return sta;
        }

        public List<SessionTableAthletes> Bark()
        {
            SessionTableAthletes athletes = new SessionTableAthletes
            {
                hour = 6,
                SessionAthletes = new string[]{
                    "asdfasdf", "qwerasdf", "zxcasdf"
                }
            };
            List<SessionTableAthletes> all = new List<SessionTableAthletes>();
            all.Add(athletes);
            return all;
        }
        
        public PartialViewResult _ChangeDateSixAm(DateTime dt)
        {
            var sixAm = getOrCreateSession(6, dt);
            SessionTableAthletes athletes = new SessionTableAthletes();

          //  athletes = sixAm.Athletes();// getSessionTableForHour(6, dt);

            return PartialView("_ChangeDateSixAm", athletes);
        }


        public PartialViewResult GetProducts()
        {
            SessionTableAthletes athletes = new SessionTableAthletes { hour = 6, SessionAthletes = new string[]{
                    "asdf", "qwer", "zxc",
                    "asdf", "qwer", "zxc",
                    "asdf", "qwer", "zxc",
                    "asdf", "qwer", "zxc",
                    "asdf", "qwer", "zxc"
                } 
            };

            return PartialView(athletes);
        }

        public ActionResult GetGreeting(string name)
        {
            return Content("Hello " + name);
        }

        // GET: Sessions/Add/5
        public ActionResult Add(int? id)
        {
            var userId = User.Identity.GetUserId();
            var appUser = db.Users.Where(u => u.Id == userId).SingleOrDefault();
            var session = db.Sessions.Where(s => s.Hour == id).SingleOrDefault();
            var athlete = db.Athletes.Where(a => a.Id == appUser.Athlete.Id).SingleOrDefault();
            session.Athletes.Add(athlete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Sessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Hour,Day")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Hour,Day")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
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

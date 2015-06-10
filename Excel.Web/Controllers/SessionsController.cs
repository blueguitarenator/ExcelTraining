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
    public class SessionsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Sessions
        public ActionResult Index()
        {
            DateTime saveNow = DateTime.Now;
            ViewBag.TodaysDate = saveNow.ToShortDateString();
            int year = saveNow.Year;
            int month = saveNow.Month;
            int day = saveNow.Day;

            List<SessionTableAthletes> sessionTableAthletes = new List<SessionTableAthletes>();
            sessionTableAthletes.Add(getSessionTableForHour(6));
            sessionTableAthletes.Add(getSessionTableForHour(7));
            sessionTableAthletes.Add(getSessionTableForHour(8));
            //sessionTableAthletes.Add(getSessionTableForHour(9));

            return View(sessionTableAthletes);
        }

        private SessionTableAthletes getSessionTableForHour(int hour)
        {
            SessionTableAthletes sta = new SessionTableAthletes();
            sta.SessionAthletes = new string[16];
            sta.hour = hour;
            sta.SessionDate = DateTime.Now;
            Session session = db.Sessions.Include(c => c.Athletes).Where(s => s.Hour == hour).SingleOrDefault();
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

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

            Session[] sessions = db.Sessions.Include(c => c.Athletes).Where(s => s.Hour == 6).ToArray();
            Session sixSession = sessions[0];
            Athlete[] athletesAndTrainer = sixSession.Athletes.ToArray();
            Athlete[] athletesOnly = athletesAndTrainer.Where(a => a.UserType == UserTypes.Athlete).ToArray();
            string[,] sixSlots = new string[4,4];
            var counter = 0;
            for(var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (counter < athletesOnly.Count())
                    {
                        sixSlots[i,j] = athletesOnly[counter].FirstName + " " + athletesOnly[counter].LastName;
                    }
                    else
                    {
                        sixSlots[i, j] = "open";
                    }
                    counter++;
                }
                    
            }

            ViewBag.SixAmAthlete = sixSlots;
            ViewBag.SixAmSession = sixSession;
            ViewBag.SevenAmAthlete = sixSlots;



            return View(db.Sessions.ToList());
        }

        // GET: Sessions/Add/5
        public ActionResult Add(int? id)
        {
            return View();
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

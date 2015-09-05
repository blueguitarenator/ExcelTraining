using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Excel.Entities;
using Excel.Web.Models;
using Microsoft.AspNet.Identity;

namespace Excel.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class MotdController : Controller
    {
        private IAthleteRepository athleteRepository;

        public Func<string> GetUserId; //For testing

        public MotdController()
        {
            athleteRepository = new AthleteRepository();
            GetUserId = () => User.Identity.GetUserId();
        }

        public MotdController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Motd
        public ActionResult Index()
        {
            var motd = athleteRepository.GetFutureMotd();
            return View(motd);
        }


        // GET: Motd/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motd motd = athleteRepository.FindMotd(id.Value);
            if (motd == null)
            {
                return HttpNotFound();
            }

            return View(motd);
        }

        // GET: Motd/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Motd/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message,DaysToLive")] Motd motd)
        {
            if (ModelState.IsValid)
            {
                RemoveCurrentMotd();
                DateTime saveNow = DateTime.Now.Date;
                for (int i = 0; i < motd.DaysToLive; i++)
                {
                    DateTime d = saveNow.AddDays(i);
                    athleteRepository.CreateMotd(new Motd {DisplayDate = d, Message = motd.Message});
                }

                return RedirectToAction("Index");
            }

            return View(motd);
        }

        private void RemoveCurrentMotd()
        {
            var motd = athleteRepository.GetFutureMotd();
            foreach (var m in motd)
            {
                athleteRepository.RemoveMotd(m);
            }
        }

        // GET: Athletes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motd motd = athleteRepository.FindMotd(id.Value);
            if (motd == null)
            {
                return HttpNotFound();
            }
            return View(motd);
        }

        // POST: Athletes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,DisplayDate")] Motd motd)
        {
            if (ModelState.IsValid)
            {
                athleteRepository.UpdateMotd(motd);
                return RedirectToAction("Index");
            }
            return View(motd);
        }

        // GET: Athletes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motd motd = athleteRepository.FindMotd(id.Value);
            if (motd == null)
            {
                return HttpNotFound();
            }
            return View(motd);
        }

        // POST: Athletes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Motd motd = athleteRepository.FindMotd(id);
            athleteRepository.RemoveMotd(motd);
            return RedirectToAction("Index");
        }
    }
}
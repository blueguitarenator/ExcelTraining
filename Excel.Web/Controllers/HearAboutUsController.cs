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
    public class HearAboutUsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: HearAboutUs
        public ActionResult Index()
        {
            return View(db.HearAboutUs.ToList());
        }

        // GET: HearAboutUs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HearAboutUs hearAboutUs = db.HearAboutUs.Find(id);
            if (hearAboutUs == null)
            {
                return HttpNotFound();
            }
            return View(hearAboutUs);
        }

        // GET: HearAboutUs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HearAboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] HearAboutUs hearAboutUs)
        {
            if (ModelState.IsValid)
            {
                db.HearAboutUs.Add(hearAboutUs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hearAboutUs);
        }

        // GET: HearAboutUs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HearAboutUs hearAboutUs = db.HearAboutUs.Find(id);
            if (hearAboutUs == null)
            {
                return HttpNotFound();
            }
            return View(hearAboutUs);
        }

        // POST: HearAboutUs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] HearAboutUs hearAboutUs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hearAboutUs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hearAboutUs);
        }

        // GET: HearAboutUs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HearAboutUs hearAboutUs = db.HearAboutUs.Find(id);
            if (hearAboutUs == null)
            {
                return HttpNotFound();
            }
            return View(hearAboutUs);
        }

        // POST: HearAboutUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HearAboutUs hearAboutUs = db.HearAboutUs.Find(id);
            db.HearAboutUs.Remove(hearAboutUs);
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

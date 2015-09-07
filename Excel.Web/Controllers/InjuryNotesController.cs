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
    public class InjuryNotesController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: InjuryNotes
        public ActionResult Index(int? athleteId)
        {
            if (athleteId.HasValue)
            {
                return View(db.InjuryNotes.Where(i => i.Athlete.Id == athleteId).ToList());
            }
            return View(db.InjuryNotes.ToList());
        }

        // GET: InjuryNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InjuryNote injuryNote = db.InjuryNotes.Find(id);
            if (injuryNote == null)
            {
                return HttpNotFound();
            }
            return View(injuryNote);
        }

        // GET: InjuryNotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InjuryNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NoteDate,Message")] InjuryNote injuryNote)
        {
            if (ModelState.IsValid)
            {
                db.InjuryNotes.Add(injuryNote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(injuryNote);
        }

        // GET: InjuryNotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InjuryNote injuryNote = db.InjuryNotes.Find(id);
            if (injuryNote == null)
            {
                return HttpNotFound();
            }
            return View(injuryNote);
        }

        // POST: InjuryNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NoteDate,Message")] InjuryNote injuryNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(injuryNote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(injuryNote);
        }

        // GET: InjuryNotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InjuryNote injuryNote = db.InjuryNotes.Find(id);
            if (injuryNote == null)
            {
                return HttpNotFound();
            }
            return View(injuryNote);
        }

        // POST: InjuryNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InjuryNote injuryNote = db.InjuryNotes.Find(id);
            db.InjuryNotes.Remove(injuryNote);
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

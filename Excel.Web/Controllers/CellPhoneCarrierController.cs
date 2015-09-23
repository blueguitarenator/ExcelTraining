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
    public class CellPhoneCarrierController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: CellPhoneCarrier
        public ActionResult Index()
        {
            return View(db.CellPhoneCarriers.ToList());
        }

        // GET: CellPhoneCarrier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CellPhoneCarrier cellPhoneCarrier = db.CellPhoneCarriers.Find(id);
            if (cellPhoneCarrier == null)
            {
                return HttpNotFound();
            }
            return View(cellPhoneCarrier);
        }

        // GET: CellPhoneCarrier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CellPhoneCarrier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] CellPhoneCarrier cellPhoneCarrier)
        {
            if (ModelState.IsValid)
            {
                db.CellPhoneCarriers.Add(cellPhoneCarrier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cellPhoneCarrier);
        }

        // GET: CellPhoneCarrier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CellPhoneCarrier cellPhoneCarrier = db.CellPhoneCarriers.Find(id);
            if (cellPhoneCarrier == null)
            {
                return HttpNotFound();
            }
            return View(cellPhoneCarrier);
        }

        // POST: CellPhoneCarrier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] CellPhoneCarrier cellPhoneCarrier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cellPhoneCarrier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cellPhoneCarrier);
        }

        // GET: CellPhoneCarrier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CellPhoneCarrier cellPhoneCarrier = db.CellPhoneCarriers.Find(id);
            if (cellPhoneCarrier == null)
            {
                return HttpNotFound();
            }
            return View(cellPhoneCarrier);
        }

        // POST: CellPhoneCarrier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CellPhoneCarrier cellPhoneCarrier = db.CellPhoneCarriers.Find(id);
            db.CellPhoneCarriers.Remove(cellPhoneCarrier);
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

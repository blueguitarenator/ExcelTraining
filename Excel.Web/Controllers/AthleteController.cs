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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Excel.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AthleteController : Controller
    {
        private IAthleteRepository athleteRepository;
        private IdentityDb db = new IdentityDb();

        // GET: Athletes
        public ActionResult Index()
        {
            AthleteViewModel model = new AthleteViewModel();
            model.Athletes = db.Athletes.ToList();
            model.Locations = db.Locations.ToList();
            return View(model);
        }

        // GET: Athletes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }

            return View(athlete);
        }


        // POST: Athletes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Address,City,State,Zip,AthleteType,UserType")] Athlete athlete)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Athletes.Add(athlete);
        //        db.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    return View(athlete);
        //}

        // GET: Athletes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            GetLocationSelectList();
            

            return View(athlete);
        }

        private void GetLocationSelectList()
        {
            var content = from p in db.Locations
                  
                  orderby p.Name
                  select new { p.Id, p.Name };
 
            var x = content.ToList().Select(c => new SelectListItem         
                            {
                               Text = c.Name,
                               Value = c.Id.ToString(),
                               
                            }).ToList();
             ViewBag.Locations = x;
 
        }

        // POST: Athletes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Address,City,State,Zip,AthleteType,UserType,LocationId")] Athlete athlete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(athlete).State = EntityState.Modified;
                if (athlete.UserType == UserTypes.Trainer)
                {
                    var store = new UserStore<ApplicationUser>(db);
                    var manager = new UserManager<ApplicationUser>(store);
                   
                    var usr = from u in db.Users 
                                    where u.Athlete.Id == athlete.Id 
                                    select u;
                    manager.AddToRole(usr.First().Id, "admin");
                    
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(athlete);
        }

        // GET: Athletes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            return View(athlete);
        }

        // POST: Athletes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Athlete athlete = db.Athletes.Find(id);
            db.Athletes.Remove(athlete);
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

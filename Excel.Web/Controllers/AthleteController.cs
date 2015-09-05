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

        public AthleteController()
        {
            athleteRepository = new AthleteRepository();
        }

        // GET: Athletes
        public ActionResult Index()
        {
            AthleteViewModel model = new AthleteViewModel();
            model.Athletes = athleteRepository.GetAllAthletes().ToList();
            return View(model);
        }

        // GET: Athletes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = athleteRepository.GetAthleteById(id.Value);
            if (athlete == null)
            {
                return HttpNotFound();
            }

            return View(athlete);
        }

        // GET: Athletes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = athleteRepository.GetAthleteById(id.Value);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            ViewBag.Locations = new SelectList(athleteRepository.GetLocations(), "Id", "Name", athlete.SelectedLocationId);
            var heardId = athlete.HearAboutUs != null ? athlete.HearAboutUsId : 0;
            ViewBag.HearAboutUs = new SelectList(athleteRepository.GetHearAboutUs(), "Id", "Name", heardId);
            return View(athlete);
        }

        // POST: Athletes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Address,City,State,Zip,AthleteType,UserType,LocationId,HearAboutUsId,SelectedLocationId,SelectedDate")] Athlete athlete)
        {
            if (ModelState.IsValid)
            {
                if (athlete.UserType == UserTypes.Trainer)
                {
                    athleteRepository.GiveAdmin(athlete);
                }
                else
                {
                    athleteRepository.RemoveAdmin(athlete);
                }
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
            Athlete athlete = athleteRepository.GetAthleteById(id.Value);
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
            //var userId = User.Identity.GetUserId();
            athleteRepository.DeleteAthlete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                athleteRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Excel.Entities;
using Excel.Web.DataContexts;
using Excel.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Excel.Web.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "admin")]
    public class AthleteController : Controller
    {
        private IAthleteRepository athleteRepository;

        public Func<string> GetUserId; //For testing

        public AthleteController()
        {
            athleteRepository = new AthleteRepository();
            GetUserId = () => User.Identity.GetUserId();
        }

        // GET: Athletes
        public ActionResult Index(string sortOrder, string searchString)
        {
            AthleteViewModel model = new AthleteViewModel();
            var allAthletes = athleteRepository.GetAllAthletes().ToList();
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";
            var athletes = from a in allAthletes
                           select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                athletes = athletes.Where(s => s.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    athletes = athletes.OrderByDescending(s => s.LastName);
                    break;
                case "Total":
                    athletes = athletes.OrderBy(s => s.SessionAthletes.Count);
                    break;
                case "total_desc":
                    athletes = athletes.OrderByDescending(s => s.SessionAthletes.Count);
                    break;
                default:
                    athletes = athletes.OrderBy(s => s.LastName);
                    break;
            }
            model.Athletes = athletes.ToList();
            return View(model);
        }

        // GET: Injury Notes
        public ActionResult InjuryIndex(int? athleteId)
        {
            if (athleteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["AthleteInjuredId"] = athleteId;
            TempData.Keep();
            ViewBag.AthleteFullName = athleteRepository.GetAthleteById(athleteId.Value).FullName;
            return View(athleteRepository.GetAthleteNotes(athleteId.Value).ToList());
        }

        // GET: Athletes/Create
        public ActionResult InjuryCreate()
        {
            ViewBag.AthleteFullName = athleteRepository.GetAthleteById((int)TempData["AthleteInjuredId"]).FullName;
            TempData.Keep();
            return View();
        }


        // POST: Athletes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InjuryCreate([Bind(Include = "Id, Message")] InjuryNote injuryNote)
        {
            if (injuryNote.Message.Length > 0 && TempData["AthleteInjuredId"] != null)
            {
                injuryNote.Athlete = athleteRepository.GetAthleteById((int)TempData["AthleteInjuredId"]);
                injuryNote.NoteDate = DateTime.Now.Date;
                athleteRepository.AddNoteToAthlete(injuryNote);
                return RedirectToAction("InjuryIndex", new {athleteId = injuryNote.Athlete.Id});
            }

            return View(injuryNote);
        }

        public ActionResult InjuryDetails(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InjuryNote note = athleteRepository.GetInjuryNote(id.Value);
            return View(note);
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
            ViewBag.Notes = athleteRepository.GetAthleteNotes(id.Value).ToList();
            return View(athlete);
        }

        // GET: Athletes/InjuryEdit/5
        public ActionResult InjuryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InjuryNote note = athleteRepository.GetInjuryNote(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Athletes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InjuryEdit([Bind(Include = "Id,Message")] InjuryNote injuryNote)
        {
            if (injuryNote.Message.Length > 0 && TempData["AthleteInjuredId"] != null)
            {
                injuryNote.Athlete = athleteRepository.GetAthleteById((int)TempData["AthleteInjuredId"]);
                injuryNote.NoteDate = DateTime.Now.Date;
                athleteRepository.UpdateNoteForAthlete(injuryNote);
                return RedirectToAction("InjuryIndex", new {athleteId = injuryNote.Athlete.Id});
            }

            return View(injuryNote);
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
        [System.Web.Mvc.HttpPost]
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
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
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

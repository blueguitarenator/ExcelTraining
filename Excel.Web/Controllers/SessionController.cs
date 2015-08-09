﻿using System;
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
using Excel.Web.Helpers;

namespace Excel.Web.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly IAthleteRepository athleteRepository;

        public Func<string> GetUserId; //For testing

        public SessionController()
        {
            athleteRepository = new AthleteRepository();
            GetUserId = () => User.Identity.GetUserId();
        }

        public SessionController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Sessions
        public ActionResult Index()
        {
            SessionModel sessionModel = new SessionModel();

            var athlete = GetThisAthlete();
            sessionModel.SessionDateTime = athlete.SelectedDate;
            sessionModel.SelectedLocationId = athlete.SelectedLocationId;

            SetAthleteType(sessionModel);

            LoadGrid(sessionModel);
            LoadLocationSelectList(sessionModel);

            return View(sessionModel);
        }

        private void SetAthleteType(SessionModel sessionModel)
        {
            if (TempData["AthleteType"] != null && TempData["AthleteType"].Equals((int) AthleteTypes.SportsTraining))
            {
                sessionModel.AthleteType = AthleteTypes.SportsTraining;
            }
            else
            {
                sessionModel.AthleteType = AthleteTypes.PersonalTraining;
            }
            TempData.Clear();
        }

        private void LoadLocationSelectList(SessionModel sessionModel)
        {
            sessionModel.LocationSelectList = new SelectList(athleteRepository.GetLocations(), "Id", "Name", sessionModel.SelectedLocationId);
        }

        public ActionResult ChangeDate(DateTime dt)
        {
            var athlete = GetThisAthlete();
            athlete.SelectedDate = dt;
            athleteRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Next()
        {
            var athlete = GetThisAthlete();
            athlete.SelectedDate = athlete.SelectedDate.AddDays(1);

            athleteRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Previous()
        {
            var athlete = GetThisAthlete();
            athlete.SelectedDate = athlete.SelectedDate.AddDays(-1);

            athleteRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeLocation(int locId)
        {
            var athlete = GetThisAthlete();
            athlete.SelectedLocationId = locId;
            athleteRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeAthleteType(int athleteTypeId)
        {
            TempData["AthleteType"] = athleteTypeId;
            return RedirectToAction("Index");
        }

        private void LoadGrid(SessionModel model)
        {
            model.SessionsWithAthletes = new SessionsWithAthletes();
            model.SessionsWithAthletes.AthleteData = new List<AthleteData>();
            if (model.AthleteType == AthleteTypes.PersonalTraining)
            {
                List<Schedule> personalTrainingSchedule = athleteRepository.GetDardenneSchedule(AthleteTypes.PersonalTraining).ToList();
                foreach (var schedule in personalTrainingSchedule)
                {
                    if (schedule.IsAvailable)
                    {
                        model.SessionsWithAthletes.AthleteData.Add(GetSessionsWithAthletes(model, schedule.Hour));
                    }
                }
            }
            else
            {
                List<Schedule> sportsTrainingSchedule = athleteRepository.GetDardenneSchedule(AthleteTypes.SportsTraining).ToList();
                foreach (var schedule in sportsTrainingSchedule)
                {
                    if (schedule.IsAvailable)
                    {
                        model.SessionsWithAthletes.AthleteData.Add(GetSessionsWithAthletes(model, schedule.Hour));
                    }
                }
            }
        }

        private AthleteData GetSessionsWithAthletes(SessionModel model, int hour)
        {
            AthleteData athleteData = new AthleteData();
            athleteData.Athletes = new List<string>();
            athleteData.Time = GetTime(hour);
            athleteData.DivId = GetDivId(hour);
            athleteData.Hour = hour;
            for (int i = 0; i < 16; i++)
            {
                athleteData.Athletes.Add("open");
            }
            GetSessionListNames(athleteData, hour, model.SessionDateTime, model.SelectedLocationId, model.AthleteType);
            return athleteData;
        }

        private void GetSessionListNames(AthleteData athleteData, int hour, DateTime dt, int locationId, AthleteTypes athleteType)
        {
            Session session = GetOrCreateSession(hour, dt, locationId, athleteType);
            
            if (session.Athletes != null)
            {
                if (athleteType == AthleteTypes.PersonalTraining)
                {
                    IEnumerable<Athlete> personal = athleteRepository.GetPersonalTrainingAthletes(session.Id, locationId);
                    for (int i = 0; i < personal.Count(); i++)
                    {
                        athleteData.Athletes[i] = personal.ElementAt(i).FirstName + " " + personal.ElementAt(i).LastName;
                    }
                }
                else
                {
                    IEnumerable<Athlete> sports = athleteRepository.GetSportsTrainingAthletes(session.Id, locationId);
                    for (int i = 0; i < sports.Count(); i++)
                    {
                        athleteData.Athletes[i] = sports.ElementAt(i).FirstName + " " + sports.ElementAt(i).LastName;
                    }
                }
            }
        }

        private Session GetOrCreateSession(int hour, DateTime dt, int locationId, AthleteTypes athleteType)
        {
            Session session = athleteRepository.GetSession(hour, dt.Date, locationId, athleteType);
            if (session == null)
            {
                athleteRepository.Write_CreateSessions(dt, locationId);
                session = athleteRepository.GetSession(hour, dt, locationId, athleteType);
            }
            return session;
        }

        // GET: Sessions/Add/5
        public PartialViewResult _OneSession(DateTime dt, int hour, int SelectedLocationId)
        {
            Athlete athlete = GetThisAthlete();
            Session session = athleteRepository.GetSession(hour, dt, SelectedLocationId, athlete.AthleteType);
            athleteRepository.AddAthleteToSession(session.Id, athlete.Id);

            AthleteData athleteData = new AthleteData();
            athleteData.Athletes = new List<string>();
            athleteData.Time = GetTime(hour);
            athleteData.DivId = GetDivId(hour);
            athleteData.Hour = hour;
            for (int i = 0; i < 16; i++)
            {
                athleteData.Athletes.Add("open");
            }
            GetSessionListNames(athleteData, hour, dt, SelectedLocationId, athlete.AthleteType);

            return PartialView(athleteData);
        }

        private Athlete GetThisAthlete()
        {
            var userId = GetUserId();
            return athleteRepository.GetAthleteByUserId(userId);
        }

        private string GetDivId(int hour)
        {
            if (hour == 6)
            {
                return "SixAm";
            }
            if (hour == 7)
            {
                return "SevenAm";
            }
            if (hour == 8)
            {
                return "EightAm";
            }
            if (hour == 9)
            {
                return "NineAm";
            }
            if (hour == 10)
            {
                return "TenAm";
            }
            if (hour == 11)
            {
                return "ElevenAm";
            }
            if (hour == 12)
            {
                return "TwelvePm";
            }
            if (hour == 13)
            {
                return "OnePm";
            }
            if (hour == 14)
            {
                return "TwoPm";
            }
            if (hour == 15)
            {
                return "ThreePm";
            }
            if (hour == 16)
            {
                return "FourPm";
            }
            if (hour == 17)
            {
                return "FivePm";
            }
            if (hour == 18)
            {
                return "SixPm";
            }
            if (hour == 19)
            {
                return "SevenPm";
            }
            if (hour == 20)
            {
                return "EightPm";
            }
            if (hour == 21)
            {
                return "NinePm";
            }
            return "";
        }

        private string GetTime(int hour)
        {
            if (hour == 6)
            {
                return "6 AM";
            }
            if (hour == 7)
            {
                return "7 AM";
            }
            if (hour == 8)
            {
                return "8 AM";
            }
            if (hour == 9)
            {
                return "9 AM";
            }
            if (hour == 10)
            {
                return "10 AM";
            }
            if (hour == 16)
            {
                return "4 PM";
            }
            if (hour == 17)
            {
                return "5 PM";
            }
            if (hour == 18)
            {
                return "6 PM";
            }
            if (hour == 19)
            {
                return "7 PM";
            }
            if (hour == 20)
            {
                return "8 PM";
            }
            if (hour == 21)
            {
                return "9 PM";
            }
            return "";
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

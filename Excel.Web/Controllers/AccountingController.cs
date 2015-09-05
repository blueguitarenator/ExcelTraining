using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel.Entities;
using Excel.Web.Models;

namespace Excel.Web.Controllers
{
    public class AccountingController : Controller
    {
        private IAthleteRepository athleteRepository;
        private QuickHelper helper = new QuickHelper();

        public AccountingController()
        {
            athleteRepository = new AthleteRepository();
            //GetUserId = () => User.Identity.GetUserId();
        }

        public AccountingController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Accounting
        public ActionResult Index()
        { 
            AccountingViewModel model = new AccountingViewModel();
            List<Session> sessions = athleteRepository.GetAllSessions().ToList();
            RemoveUnconfirmed(sessions);
            
            model.PersonalTrainingSessionsWithAthletes = GetAllSessions(sessions);
            return View(model);
        }

        private List<DaySession> GetAllSessions(List<Session> sessions)
        {
            SortedDictionary<DateTime, List<Session>> sessionDictionary = 
                new SortedDictionary<DateTime, List<Session>>(Comparer<DateTime>.Create((x, y) => y.CompareTo(x)));
            foreach (var session in sessions)
            {
                if (session.SessionAthletes.Count > 0)
                {
                    if (sessionDictionary.ContainsKey(session.Day.Date))
                    {
                        sessionDictionary[session.Day.Date].Add(session);
                    }
                    else
                    {
                        List<Session> sessionList = new List<Session> { session };
                        sessionDictionary[session.Day.Date] = sessionList;
                    }
                }
            }
            List<DaySession> daySessionList = new List<DaySession>();
            foreach (KeyValuePair<DateTime, List<Session>> entry in sessionDictionary)
            {
                DateTime d = entry.Key;
                List<Session> oneDaysSessions = entry.Value;
                DaySession daySession = new DaySession();
                daySession.Date = d.ToLongDateString();
                List<SingleSession> singleSessions = new List<SingleSession>();
                foreach (var session in oneDaysSessions)
                {
                    SingleSession singleSession = new SingleSession();
                    singleSession.Athletes = athleteRepository.GetConfirmedPersonalTrainingAthletes(session.Id, 1).ToList();
                    singleSession.Hour = session.Hour;
                    singleSessions.Add(singleSession);
                }
                daySession.DaySessionList = singleSessions;
                daySessionList.Add(daySession);
            }
            return daySessionList;
        } 

        private void RemoveUnconfirmed(List<Session> sessions)
        {
            foreach (var session in sessions)
            {
                List<SessionAthlete> athletes = session.SessionAthletes.ToList();
                foreach (var sessionAthlete in athletes)
                {
                    if (!sessionAthlete.Confirmed)
                    {
                        athleteRepository.RemoveAthleteFromSession(session.Id, sessionAthlete.AthleteId);
                    }
                }
            }
        }
    }
}
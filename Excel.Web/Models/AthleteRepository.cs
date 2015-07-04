using Excel.Entities;
using Excel.Web.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Security.Principal;

namespace Excel.Web.Models
{
    public class AthleteRepository : IAthleteRepository
    {
        private IdentityDb db = new IdentityDb();

        public void CreateNewAthlete(Athlete athleteToCreate)
        {
            db.Athletes.Add(athleteToCreate);
            db.SaveChanges();
        }

        public void DeleteAthlete(int id)
        {
            var athleteToDelete = GetAthleteById(id);
            db.Athletes.Remove(athleteToDelete);
            db.SaveChanges();
        }

        public Athlete GetAthleteById(int id)
        {
            return db.Athletes.FirstOrDefault(d => d.Id == id);
        }

        public Athlete GetAthleteByUserId(string userId)
        {
            var appUser = db.Users.Where(u => u.Id == userId).SingleOrDefault();
            return db.Athletes.FirstOrDefault(d => d.Id == appUser.Athlete.Id);
        }

        public Session GetSessionById(int id)
        {
            return db.Sessions.Where(s => s.Id == id).FirstOrDefault();
        }

        public IEnumerable<Athlete> GetAllAthletes()
        {
            return db.Athletes.ToList();
        }

        public void RemoveAthleteFromSession(int sessionId, int athleteId)
        {
            var session = GetSessionById(sessionId);
            var athlete = GetAthleteById(athleteId);
            var x = session.Athletes.Where(a => a.Id == athlete.Id).SingleOrDefault();

            session.Athletes.Remove(x);
        }

        public IEnumerable<Session> GetFutureSessions(int athleteId)
        {
            DateTime saveNow = DateTime.Now.Date;

            var q = from s in db.Sessions
                    where s.Day.Year >= saveNow.Year &&
                        s.Day.Month >= saveNow.Month &&
                        s.Day.Day >= saveNow.Day &&
                        s.Athletes.Any(a => a.Id == athleteId)
                    select s;
            return q;
        }

        public IEnumerable<Session> GetPastSessions(int athleteId)
        {
            DateTime saveNow = DateTime.Now.Date;
            var q = from s in db.Sessions
                    where s.Day.Year <= saveNow.Year &&
                        s.Day.Month <= saveNow.Month &&
                        s.Day.Day < saveNow.Day &&
                        s.Athletes.Any(a => a.Id == athleteId)
                    select s;

            return q;
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}
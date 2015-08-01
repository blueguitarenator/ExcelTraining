﻿using Excel.Entities;
using Excel.Web.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Security.Principal;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Excel.Web.Models
{
    public class AthleteRepository : IAthleteRepository
    {
        private IdentityDb db = new IdentityDb();

        //Athlete
        public void GiveAdmin(Athlete athlete)
        {
            db.Entry(athlete).State = EntityState.Modified;
            var store = new UserStore<ApplicationUser>(db);
            var manager = new UserManager<ApplicationUser>(store);

            var usr = from u in db.Users
                      where u.Athlete.Id == athlete.Id
                      select u;
            manager.AddToRole(usr.First().Id, "admin");
            db.SaveChanges();
        }

        public void RemoveAdmin(Athlete athlete)
        {
            db.Entry(athlete).State = EntityState.Modified;
            var store = new UserStore<ApplicationUser>(db);
            var manager = new UserManager<ApplicationUser>(store);

            var usr = from u in db.Users
                      where u.Athlete.Id == athlete.Id
                      select u;
            manager.RemoveFromRole(usr.First().Id, "admin");
            db.SaveChanges();
        }

        public void CreateNewAthlete(Athlete athleteToCreate)
        {
            db.Athletes.Add(athleteToCreate);
            db.SaveChanges();
        }

        public void DeleteAthlete(int id)
        {
            var athleteToDelete = GetAthleteById(id);
            db.Entry(athleteToDelete).State = EntityState.Modified;
            var store = new UserStore<ApplicationUser>(db);
            var manager = new UserManager<ApplicationUser>(store);
            
            var usr = from u in db.Users
                      where u.Athlete.Id == id
                      select u;
            manager.RemoveFromRole(usr.First().Id, "admin");
            manager.Delete(usr.FirstOrDefault());
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

        public Athlete GetAthleteByEmail(string email)
        {
            var appUser = db.Users.Where(u => u.Email == email).SingleOrDefault();
            if (appUser != null)
            {
                return db.Athletes.FirstOrDefault(d => d.Id == appUser.Athlete.Id);
            }
            return null;
        }

        public IEnumerable<Athlete> GetAllAthletes()
        {
            return db.Athletes.ToList();
        }

        public IEnumerable<Athlete> GetAllTrainers()
        {
            return db.Athletes.ToList().Where(t => t.UserType == UserTypes.Trainer);
        }

        public void RemoveAthleteFromSession(int sessionId, int athleteId)
        {
            var session = GetSessionById(sessionId);
            var athlete = GetAthleteById(athleteId);
            var x = session.Athletes.Where(a => a.Id == athlete.Id).SingleOrDefault();

            session.Athletes.Remove(x);
        }

        public void AddAthleteToSession(int sessionId, int athleteId)
        {
            var session = GetSessionById(sessionId);
            var athlete = GetAthleteById(athleteId);
            session.Athletes.Add(athlete);
            db.SaveChanges();
        }

        // Sessions
        public Session GetSession(int hour, DateTime dt, int locationId, AthleteTypes athleteType)
        {
            Session session = db.Sessions.Where(
                s => s.Hour == hour &&
                s.Day.Year == dt.Year &&
                s.Day.Month == dt.Month &&
                s.Day.Day == dt.Day &&
                s.LocationId == locationId &&
                s.AthleteType == athleteType).SingleOrDefault();

            return session;
        }

        public void Write_CreateSessions(DateTime dt, int locationId)
        {
            IEnumerable<Schedule> personalTrainingSchedule = db.Schedules.Where(s => s.AthleteType == AthleteTypes.PersonalTraining);
            foreach (var schedule in personalTrainingSchedule)
            {
                if (schedule.IsAvailable)
                {
                    db.Sessions.Add(new Session { Day = dt, Hour = schedule.Hour, LocationId = locationId, AthleteType = AthleteTypes.PersonalTraining });
                }
            }
            IEnumerable<Schedule> sportsTrainingSchedule = db.Schedules.Where(s => s.AthleteType == AthleteTypes.SportsTraining);
            foreach (var schedule in sportsTrainingSchedule)
            {
                if (schedule.IsAvailable)
                {
                    db.Sessions.Add(new Session { Day = dt, Hour = schedule.Hour, LocationId = locationId, AthleteType = AthleteTypes.SportsTraining });
                }
            }

            db.SaveChanges();
        }

        public Session GetSessionById(int id)
        {
            return db.Sessions.Where(s => s.Id == id).FirstOrDefault();
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

        public IEnumerable<Athlete> GetPersonalTrainingAthletes(int sessionId, int locationId)
        {
            var session = GetSessionById(sessionId);
            return session.Athletes.Where(a => a.AthleteType == AthleteTypes.PersonalTraining && a.UserType == UserTypes.Athlete && a.LocationId == locationId);
        }

        public IEnumerable<Athlete> GetSportsTrainingAthletes(int sessionId, int locationId)
        {
            var session = GetSessionById(sessionId);
            return session.Athletes.Where(a => a.AthleteType == AthleteTypes.SportsTraining && a.UserType == UserTypes.Athlete && a.LocationId == locationId);
        }

        //Locations
        public IEnumerable<Location> GetLocations()
        {
            return db.Locations;
        }

        //UserTypes
        public IEnumerable<AthleteTypes> GetAthleteTypes()
        {
            List<AthleteTypes> all = new List<AthleteTypes>() {AthleteTypes.PersonalTraining, AthleteTypes.SportsTraining};
            return all.AsEnumerable();
        }

        public IEnumerable<Schedule> GetDardenneSchedule(AthleteTypes athleteType)
        {
            return db.Schedules.Where(s => s.Location.Name.Contains("Dardenne") && s.AthleteType == athleteType);
        }

        public IdentityDb GetIdentityDb()
        {
            return db;
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
using Excel.Entities;
using Excel.Web.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Security.Principal;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Excel.Web.Models
{
    public class AthleteRepository : IAthleteRepository
    {
        private IdentityDb db;

        public AthleteRepository()
        {
            db = new IdentityDb();
        }

        public AthleteRepository(DbContext db)
        {
            this.db = db as IdentityDb;
        }

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

        public void AddNoteToAthlete(InjuryNote injuryNote)
        {
            var athlete = db.Athletes.First(a => a.Id == injuryNote.Athlete.Id);
            athlete.InjuryNotes.Add(injuryNote);
            db.SaveChanges();
        }

        public void UpdateNoteForAthlete(InjuryNote injuryNote)
        {
            db.InjuryNotes.AddOrUpdate(injuryNote);
            db.SaveChanges();
        }


        public InjuryNote GetInjuryNote(int id)
        {
            return db.InjuryNotes.First(i => i.Id == id);
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
            if (usr.FirstOrDefault() != null)
            {
                manager.RemoveFromRole(usr.First().Id, "admin");
                manager.Delete(usr.FirstOrDefault());
            }
            db.Athletes.Remove(athleteToDelete);
            db.SaveChanges();
        }

        public Athlete GetAthleteById(int id)
        {
            return db.Athletes.FirstOrDefault(d => d.Id == id);
        }

        public Athlete GetAthleteByUserId(string userId)
        {
            var appUser = db.Users.SingleOrDefault(u => u.Id == userId);
            return db.Athletes.FirstOrDefault(d => d.Id == appUser.Athlete.Id);
        }

        public Athlete GetAthleteByEmail(string email)
        {
            var appUser = db.Users.SingleOrDefault(u => u.Email == email);
            if (appUser != null)
            {
                return db.Athletes.FirstOrDefault(d => d.Id == appUser.Athlete.Id);
            }
            return null;
        }

        public IEnumerable<InjuryNote> GetAthleteNotes(int athleteId)
        {
            return db.InjuryNotes.Where(i => i.Athlete.Id == athleteId);
        }

        public IEnumerable<Athlete> GetAllAthletes()
        {
            return db.Athletes.Where(a => a.UserType == UserTypes.Athlete).OrderBy(a => a.LastName);
        }

        public IEnumerable<Athlete> GetAllTrials()
        {
            return db.Athletes.Where(a => a.UserType == UserTypes.Trial).OrderBy(a => a.LastName);
        }

        public IEnumerable<Athlete> GetAllTrainers()
        {
            return db.Athletes.Where(t => t.UserType == UserTypes.Trainer);
        }

        public void ConfirmAthlete(int sessionId, int athleteId)
        {
            var firstOrDefault = db.SessionAthletes.FirstOrDefault(sa => sa.SessionId == sessionId && sa.AthleteId == athleteId);
            if (firstOrDefault != null)
            {
                firstOrDefault.Confirmed = true;
                db.SaveChanges();
            }
        }

        public bool IsAthleteConfirmed(int sessionId, int athleteId)
        {
            var firstOrDefault = db.SessionAthletes.FirstOrDefault(sa => sa.SessionId == sessionId && sa.AthleteId == athleteId);
            return firstOrDefault != null && firstOrDefault.Confirmed;
        }

        public void RemoveAthleteFromSession(int sessionId, int athleteId)
        {
            var q = db.SessionAthletes.FirstOrDefault(sa => sa.SessionId == sessionId && sa.AthleteId == athleteId);
            db.SessionAthletes.Remove(q);
            db.SaveChanges();
        }

        public void AddAthleteToSession(int sessionId, int athleteId)
        {
            var sessionAthlete = new SessionAthlete {SessionId = sessionId, AthleteId = athleteId, Confirmed = false};
            db.SessionAthletes.Add(sessionAthlete);
            db.SaveChanges();
        }

        // Sessions
        public IEnumerable<Session> GetAllSessions()
        {
            DateTime saveNow = DateTime.Now.Date;
            return db.Sessions.Where(s => s.Day <= saveNow).OrderByDescending(s => s.Day);
        }

        public Session GetSession(int hour, DateTime dt, int locationId, AthleteTypes athleteType)
        {
            Session session = db.Sessions.SingleOrDefault(s => s.Hour == hour &&
                s.Day.Year == dt.Year &&
                s.Day.Month == dt.Month &&
                s.Day.Day == dt.Day &&
                s.LocationId == locationId &&
                s.AthleteType == athleteType);

            return session;
        }

        public void Write_CreateSessions(DateTime dt, int locationId)
        {
            List<Schedule> personalTrainingSchedule = db.Schedules.Where(s => s.AthleteType == AthleteTypes.PersonalTraining).ToList();
            foreach (var schedule in personalTrainingSchedule)
            {
                if (schedule.IsAvailable)
                {
                    var session = new Session { Day = dt, Hour = schedule.Hour, LocationId = locationId, AthleteType = AthleteTypes.PersonalTraining };
                    if (!DoesExist(session, AthleteTypes.PersonalTraining))
                    {
                        db.Sessions.Add(new Session { Day = dt, Hour = schedule.Hour, LocationId = locationId, AthleteType = AthleteTypes.PersonalTraining });
                    }
                }
            }
            List<Schedule> sportsTrainingSchedule = db.Schedules.Where(s => s.AthleteType == AthleteTypes.SportsTraining).ToList();
            foreach (var schedule in sportsTrainingSchedule)
            {
                if (schedule.IsAvailable)
                {
                    var session = new Session { Day = dt, Hour = schedule.Hour, LocationId = locationId, AthleteType = AthleteTypes.SportsTraining };
                    if (!DoesExist(session, AthleteTypes.SportsTraining))
                    {
                        db.Sessions.Add(session);
                    }
                }
            }

            db.SaveChanges();
        }

        private bool DoesExist(Session session, AthleteTypes athleteType)
        {
            return GetSession(session.Hour, session.Day, session.LocationId, athleteType) != null;
        }

        public Session GetSessionById(int id)
        {
            return db.Sessions.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Session> GetFutureSessions(int athleteId)
        {
            
            DateTime saveNow = DateTime.Now.Date;

            return
                db.Sessions.Where(
                    s => s.SessionAthletes.Any(sa => sa.AthleteId == athleteId) && s.Day >= saveNow);
        }

        public IEnumerable<Session> GetPastSessions(int athleteId)
        {
            DateTime saveNow = DateTime.Now.Date;
            
            return
                db.Sessions.Where(
                    s => s.SessionAthletes.Any(sa => sa.AthleteId == athleteId) && s.Day <= saveNow);
        }

        public IEnumerable<Athlete> GetConfirmedPersonalTrainingAthletes(int sessionId, int locationId)
        {
            return db.Athletes.Where(a => a.AthleteType == AthleteTypes.PersonalTraining && a.SessionAthletes.Any(sa => sa.SessionId == sessionId && sa.Confirmed));
        }

        public IEnumerable<Athlete> GetPersonalTrainingAthletes(int sessionId, int locationId)
        {
            return db.Athletes.Where(a => a.AthleteType == AthleteTypes.PersonalTraining && a.SessionAthletes.Any(sa => sa.SessionId == sessionId));
        }

        public IEnumerable<Athlete> GetSportsTrainingAthletes(int sessionId, int locationId)
        {
            return db.Athletes.Where(a => a.AthleteType == AthleteTypes.SportsTraining && a.SessionAthletes.Any(sa => sa.SessionId == sessionId));
        }

        public Athlete GetSesssionTrainer(int sessionId)
        {
            return db.Athletes.FirstOrDefault(a => a.UserType == UserTypes.Trainer && a.SessionAthletes.Any(sa => sa.SessionId == sessionId));
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

        // Schedules
        public IEnumerable<Schedule> GetDardenneSchedule(AthleteTypes athleteType)
        {
            return db.Schedules.Where(s => s.Location.Name.Contains("Dardenne") && s.AthleteType == athleteType && s.IsAvailable);
        }

        public Schedule GetScheduleById(int scheduleId)
        {
            return db.Schedules.First(s => s.Id == scheduleId);
        }

        public void SetScheduleStatus(int scheduleId, bool status)
        {
            Schedule schedule = db.Schedules.FirstOrDefault(s => s.Id == scheduleId);
            if (schedule != null)
            {
                schedule.IsAvailable = status;
                schedule.Location = GetLocations().FirstOrDefault(s => s.Name.Contains("Dardenne"));
            }
            DoSaveChanges();
        }

        // MOTD
        public void CreateMotd(Motd msg)
        {
            db.Motd.Add(msg);
            db.SaveChanges();
        }

        public Motd GetMotd()
        {
            var saveNow = DateTime.Now;
            return db.Motd.FirstOrDefault(m => DbFunctions.TruncateTime(m.DisplayDate) == saveNow.Date);
        }

        public List<Motd> GetFutureMotd()
        {
            var saveNow = DateTime.Now;
            return db.Motd.Where(m => DbFunctions.TruncateTime(m.DisplayDate) >= saveNow.Date).ToList();
        }

        public Motd FindMotd(int id)
        {
            return db.Motd.Find(id);
        }

        public void UpdateMotd(Motd motd)
        {
            db.Entry(motd).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void RemoveMotd(Motd motd)
        {
            db.Motd.Remove(motd);
            db.SaveChanges();
        }

        public IEnumerable<HearAboutUs> GetHearAboutUs()
        {
            return db.HearAboutUs;
        }

        public IEnumerable<CellPhoneCarrier> GetCellPhoneCarriers()
        {
            return db.CellPhoneCarriers;
        }

        public IdentityDb GetIdentityDb()
        {
            return db;
        }

        public int SaveChanges()
        {
            return DoSaveChanges();
        }

        private int DoSaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
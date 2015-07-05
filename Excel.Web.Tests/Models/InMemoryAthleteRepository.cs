using Excel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel.Entities;
using System.Security.Principal;

namespace Excel.Web.Tests.Models
{
    public class InMemoryAthleteRepository 
    {
        private List<Athlete> athleteDb = new List<Athlete>();
        private List<Session> sessionDb = new List<Session>();

        public Exception ExceptionToThrow { get; set; }

        public InMemoryAthleteRepository(List<Athlete> athletes)
        {
            athleteDb = athletes;
        }

        //public void SaveChanges(Athlete athleteToUpdate)
        //{

        //    foreach (Athlete athlete in athleteDb)
        //    {
        //        if (athlete.Id == athleteToUpdate.Id)
        //        {
        //            athleteDb.Remove(athlete);
        //            athleteDb.Add(athleteToUpdate);
        //            break;
        //        }
        //    }
        //}

        public void Add(Athlete athleteToAdd)
        {
            
            athleteDb.Add(athleteToAdd);
        }

        public Athlete GetAthleteById(int id)
        {
            return athleteDb.FirstOrDefault(d => d.Id == id);
        }

        public Athlete GetAthleteByUserId(string userId)
        {
            return athleteDb.FirstOrDefault();
        }

        public void CreateNewAthlete(Athlete athleteToCreate)
        {
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

            athleteDb.Add(athleteToCreate);
            // return contactToCreate;
        }

        public int SaveChanges()
        {
            return 1;
        }

        public IEnumerable<Athlete> GetAllAthletes()
        {
            return athleteDb.ToList();
        }

        public Session GetSessionById(int id)
        {
            return null;// db.Sessions.Where(s => s.Id == id).FirstOrDefault();
        }

        public void RemoveAthleteFromSession(int sessionId, int athleteId)
        {

        }

        public IEnumerable<Session> GetPastSessions(int athleteId)
        {
            List<Session> sessions = new List<Session>();
            return sessions;
        }

        public IEnumerable<Session> GetFutureSessions(int athleteId)
        {
            List<Session> sessions = new List<Session>();
            return sessions;
        }

        public void DeleteAthlete(int id)
        {
            athleteDb.Remove(GetAthleteById(id));
        }

    }
}

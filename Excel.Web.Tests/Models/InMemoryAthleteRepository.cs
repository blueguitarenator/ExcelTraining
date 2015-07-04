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
    public class InMemoryAthleteRepository : IAthleteRepository
    {
        private List<Athlete> db = new List<Athlete>();
        //private List<IPrincipal> userDb = new List<IPrincipal>();

        public Exception ExceptionToThrow { get; set; }

        public InMemoryAthleteRepository(List<Athlete> athletes)
        {
            db = athletes;
        }

        public void SaveChanges(Athlete athleteToUpdate)
        {

            foreach (Athlete athlete in db)
            {
                if (athlete.Id == athleteToUpdate.Id)
                {
                    db.Remove(athlete);
                    db.Add(athleteToUpdate);
                    break;
                }
            }
        }

        public void Add(Athlete athleteToAdd)
        {
            
            db.Add(athleteToAdd);
        }

        public Athlete GetAthleteById(int id)
        {
            return db.FirstOrDefault(d => d.Id == id);
        }

        public Athlete GetAthleteByUserId(string userId)
        {
            return db.FirstOrDefault();
        }

        public void CreateNewAthlete(Athlete athleteToCreate)
        {
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

            db.Add(athleteToCreate);
            // return contactToCreate;
        }

        public int SaveChanges()
        {
            return 1;
        }

        public IEnumerable<Athlete> GetAllAthletes()
        {
            return db.ToList();
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
            db.Remove(GetAthleteById(id));
        }

    }
}

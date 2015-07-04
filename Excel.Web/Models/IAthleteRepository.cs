using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public interface IAthleteRepository
    {
        void CreateNewAthlete(Athlete athleteToCreate);
        void DeleteAthlete(int id);
        Athlete GetAthleteById(int id);
        Athlete GetAthleteByUserId(string id);
        IEnumerable<Athlete> GetAllAthletes();
        Session GetSessionById(int id);
        void RemoveAthleteFromSession(int sessionId, int athleteId);
        IEnumerable<Session> GetFutureSessions(int athleteId);
        IEnumerable<Session> GetPastSessions(int athleteId);
        int SaveChanges();
    }
}
﻿using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public interface IAthleteRepository
    {
        //Athlete - writers
        void CreateNewAthlete(Athlete athleteToCreate);
        void DeleteAthlete(int id);
        void RemoveAthleteFromSession(int sessionId, int athleteId);
        void AddAthleteToSession(int sessionId, int athleteId);

        //Athlete - readers
        Athlete GetAthleteById(int id);
        Athlete GetAthleteByUserId(string id);
        IEnumerable<Athlete> GetAllAthletes();

        //Session - readers
        IEnumerable<Session> GetFutureSessions(int athleteId);
        IEnumerable<Session> GetPastSessions(int athleteId);
        Session GetSessionById(int id);
        Session GetSession(int hour, DateTime dt, int locationId);
        IEnumerable<Athlete> GetPersonalTrainingAthletes(int sessionId);

        //Session - writers
        void Write_CreateSessions(DateTime dt, int locationId);

        //Location
        IEnumerable<Location> GetLocations();

        int SaveChanges();
        void Dispose();
    }
}
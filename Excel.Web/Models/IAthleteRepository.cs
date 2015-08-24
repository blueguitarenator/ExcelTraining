﻿using Excel.Entities;
using Excel.Web.DataContexts;
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
        void GiveAdmin(Athlete athlete);
        void RemoveAdmin(Athlete athlete);
        void ConfirmAthlete(int sessionId, int athleteId);

        //Athlete - readers
        Athlete GetAthleteById(int id);
        Athlete GetAthleteByUserId(string id);
        Athlete GetAthleteByEmail(string email);
        IEnumerable<Athlete> GetAllAthletes();
        IEnumerable<Athlete> GetAllTrainers();
        bool IsAthleteConfirmed(int sessionId, int athleteId);

        //Session - readers
        IEnumerable<Session> GetFutureSessions(int athleteId);
        IEnumerable<Session> GetPastSessions(int athleteId);
        Session GetSessionById(int id);
        Session GetSession(int hour, DateTime dt, int locationId, AthleteTypes athleteType);
        IEnumerable<Athlete> GetPersonalTrainingAthletes(int sessionId, int locationId);
        IEnumerable<Athlete> GetSportsTrainingAthletes(int sessionId, int locationId);
        Athlete GetSesssionTrainer(int sessionId);

        //Session - writers
        void Write_CreateSessions(DateTime dt, int locationId);

        //Location
        IEnumerable<Location> GetLocations();

        // Athlete Type
        IEnumerable<AthleteTypes> GetAthleteTypes();

        // Schedules - reader
        IEnumerable<Schedule> GetDardenneSchedule(AthleteTypes athleteType);
        

        // Schedules - writers
        void SetScheduleStatus(int scheduleId, bool status);

        IdentityDb GetIdentityDb();
        int SaveChanges();
        void Dispose();
        
    }
}
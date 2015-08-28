using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class QuickHelper
    {
        public string GetSessionTimeString(DateTime nextSession)
        {
            string sessionTime = "";
            if (nextSession.Hour < 12)
            {
                sessionTime = nextSession.Hour.ToString() + ":00 AM";
            }
            else
            {
                int hour = nextSession.Hour - 12;
                sessionTime = hour.ToString() + ":00 PM";
            }
            return sessionTime;
        }

        public DateTime GetNextSession(IAthleteRepository repo)
        {
            DateTime nextSession = GetCentralStandardTimeNow();

            bool isFuture = false;
            while (!isSessionTime(nextSession, repo))
            {
                isFuture = true;
                nextSession = nextSession.AddHours(1);
            }
            if (!isFuture && nextSession.Minute > 15)
            {
                nextSession = nextSession.AddHours(1);
            }
            return nextSession;
        }

        public DateTime GetNextSession(DateTime session, IAthleteRepository repo)
        {
            DateTime nextSession = session.AddHours(1);

            bool isFuture = false;
            while (!isSessionTime(nextSession, repo))
            {
                isFuture = true;
                nextSession = nextSession.AddHours(1);
            }
            //if (!isFuture && nextSession.Minute > 15)
            //{
            //    nextSession = nextSession.AddHours(1);
            //}
            return nextSession;
        }

        public DateTime GetPreviousSession(DateTime session, IAthleteRepository repo)
        {
            DateTime nextSession = session.AddHours(-1);

            bool isPast = false;
            while (!isSessionTime(nextSession, repo))
            {
                isPast = true;
                nextSession = nextSession.AddHours(-1);
            }
            //if (!isPast && nextSession.Minute > 15)
            //{
            //    nextSession = nextSession.AddHours(1);
            //}
            return nextSession;
        }

        public Location GetDardenne(IAthleteRepository repo)
        {
            IEnumerable<Location> locationList = repo.GetLocations();
            var enumerable = locationList as IList<Location> ?? locationList.ToList();
            Location loc = enumerable.ElementAt(0);
            if (loc.Name.Contains("Dardenne"))
            {
                return loc;
            }
            loc = enumerable.ElementAt(1);
            if (loc.Name.Contains("Dardenne"))
            {
                return loc;
            }
            return null;
        }
            
        public Session GetOrCreateSession(int hour, DateTime dt, int locationId, AthleteTypes athleteType, IAthleteRepository repo)
        {
            Session session = repo.GetSession(hour, dt, locationId, athleteType);
            if (session == null)
            {
                repo.Write_CreateSessions(dt, locationId);
                session = repo.GetSession(hour, dt, locationId, athleteType);
            }
            return session;
        }

        private DateTime GetCentralStandardTimeNow()
        {
            DateTime timeUtc = DateTime.UtcNow;
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                Console.WriteLine("The date and time are {0} {1}.", cstTime,
                                  cstZone.IsDaylightSavingTime(cstTime) ? cstZone.DaylightName : cstZone.StandardName);
                return cstTime;
            }
            catch (TimeZoneNotFoundException)
            {
                Console.WriteLine("The registry does not define the Central Standard Time zone.");
            }
            catch (InvalidTimeZoneException)
            {
                Console.WriteLine("Registry data on the Central STandard Time zone has been corrupted.");
            }
            return timeUtc;
        }

        private bool isSessionTime(DateTime nextSession, IAthleteRepository repo)
        {
            List<Schedule> schedules = repo.GetDardenneSchedule(AthleteTypes.PersonalTraining).ToList();
            bool status = nextSession.DayOfWeek != DayOfWeek.Saturday &&
                          nextSession.DayOfWeek != DayOfWeek.Sunday;
            if (status)
            {
                foreach (var schedule in schedules)
                {
                    if (schedule.IsAvailable && schedule.Hour == nextSession.Hour)
                    {
                        return true;
                    }
                }
            }
            return false;
            //return nextSession.DayOfWeek != DayOfWeek.Saturday &&
            //    nextSession.DayOfWeek != DayOfWeek.Sunday &&
            //    (nextSession.Hour > 5 && nextSession.Hour < 11 ||
            //    nextSession.Hour > 15 && nextSession.Hour < 18);
        }
    }
}
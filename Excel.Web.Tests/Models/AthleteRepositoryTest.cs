using Excel.Entities;
using Excel.Web.DataContexts;
using Excel.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Web.Tests.Models
{

    [TestClass]
    public class AthleteRepositoryTest
    {
        private AthleteRepository testObject;
        private Mock<IdentityDb> mockContext;
        private Mock<DbSet<Athlete>> mockSetAthlete;
        private Mock<DbSet<Session>> mockSetSession;
        private Mock<DbSet<Location>> mockSetLocation;
        private Mock<DbSet<Schedule>> mockSetSchedule;
        private Mock<DbSet<SessionAthlete>> mockSetSessionAthlete;

        private Athlete paul;
        private Athlete john;
        private Athlete george;
        private Athlete ringo;

        private Session aug18_6, aug18_7, aug18_8, aug18_9, aug18_10, aug18_16, aug18_17, aug18_18, aug18_19;
        private Session aug19_6, aug19_7, aug19_8, aug19_9, aug19_10, aug19_16, aug19_17, aug19_18, aug19_19;
        IQueryable<Session> sessions;

        [TestInitialize]
        public void setup()
        {
            testObject = SetupData();
        }

        [TestMethod]
        public void testGetAthleteById()
        {
            var athlete = testObject.GetAthleteById(111);
            Assert.AreEqual("Paul", athlete.FirstName);
        }

        [TestMethod]
        public void testCreateAthlete()
        {
            var athlete = new Athlete { FirstName = "Test", LastName = "Test" };

            testObject.CreateNewAthlete(athlete);

            mockSetAthlete.Verify(m => m.Add(athlete));
            mockContext.Verify(db => db.SaveChanges());
        }

        [TestMethod]
        public void testGetAllTrainers()
        {
            var trainers = testObject.GetAllTrainers();
            Assert.AreEqual(3, trainers.Count());
        }

        [TestMethod]
        public void testGetAllAthletes()
        {
            var athletes = testObject.GetAllAthletes();
            Assert.AreEqual(4, athletes.Count());
        }

        [TestMethod]
        public void testGetSessionById()
        {
            var session = testObject.GetSessionById(1);
            Assert.AreEqual(6, session.Hour);
        }


        //[TestMethod]
        //public void testAddAthleteToSession()
        //{
        //    var theSession = sessions.Where(s => s.Id == 1).FirstOrDefault();
        //    Assert.AreEqual(3, theSession.Athletes.Count());
        //    testObject.AddAthleteToSession(theSession.Id, ringo.Id);
        //    Assert.AreEqual(4, theSession.Athletes.Count());
        //}

        // TODO:  keep back filling : )

        private AthleteRepository SetupData()
        {
            var locations = new List<Location>
            {
                new Location {Name = "Dardenne"}, 
                new Location {Name = "Mid Rivers"}
            }.AsQueryable();
            Location dardenne = locations.ElementAt(0);
            DateTime aug18 = new DateTime(2015, 08, 18);
            DateTime aug19 = new DateTime(2015, 08, 19);
            var athletes = new List<Athlete> 
            { 
                new Athlete { Id = 1, FirstName = "Kenny", LastName = "Ball", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = aug18 }, 
                new Athlete { Id = 2, FirstName = "Rich", LastName = "Schwepker", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = aug18 }, 
                new Athlete { Id = 3, FirstName = "Erin", LastName = "Sitz", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = aug18 }, 
                new Athlete { Id = 111, FirstName = "Paul", LastName = "McCartney", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = aug18 }, 
                new Athlete { Id = 222, FirstName = "John", LastName = "Lennon", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = aug18 }, 
                new Athlete { Id = 333, FirstName = "George", LastName = "Harrison", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = aug18 }, 
                new Athlete { Id = 444, FirstName = "Ringo", LastName = "Starr", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = aug18 }, 
            }.AsQueryable();
            paul = athletes.ElementAt(3);
            john = athletes.ElementAt(4);
            george = athletes.ElementAt(5);
            ringo = athletes.ElementAt(6);

            sessions = MakeListOne(aug18, dardenne);
            sessions = sessions.Concat(MakeListTwo(aug19, dardenne));

            //sessions = new List<Session>
            //{
            //    new Session{ Id = 1, Hour =6, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{ Id = 2, Hour =7, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{ Id = 3, Hour =8, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{ Id = 4, Hour =16, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 5, Hour =17, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 6, Hour =18, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 7, Hour =19, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 8, Hour =20, Day =aug18, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //}.AsQueryable();

            var paulSession1 = new SessionAthlete { SessionId = aug18_6.Id, AthleteId = paul.Id };
            var paulSession2 = new SessionAthlete { SessionId = aug19_6.Id, AthleteId = paul.Id };
            var johnSession1 = new SessionAthlete { SessionId = aug18_6.Id, AthleteId = paul.Id };
            var johnSession2 = new SessionAthlete { SessionId = aug19_6.Id, AthleteId = paul.Id };
            var allSessionAthletes = new List<SessionAthlete>
            {
                paulSession1,
                paulSession2,
                johnSession1,
                johnSession2
            }.AsQueryable();


            var schedules = new List<Schedule>
            {
                new Schedule { Hour = 6, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 7, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 8, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 9, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 10, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 11, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 12, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 13, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 14, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 15, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 16, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 17, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 18, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 19, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 20, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 21, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.PersonalTraining },
                new Schedule { Hour = 6, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 7, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 8, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 9, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 10, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 11, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 12, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 13, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 14, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 15, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 16, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 17, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 18, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 19, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 20, IsAvailable = true, Location = dardenne, AthleteType = AthleteTypes.SportsTraining },
                new Schedule { Hour = 21, IsAvailable = false, Location = dardenne, AthleteType = AthleteTypes.SportsTraining }
            }.AsQueryable();

            // Athlete
            mockSetAthlete = new Mock<DbSet<Athlete>>();
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.Provider)
                   .Returns(athletes.Provider);
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.Expression)
                   .Returns(athletes.Expression);
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.ElementType)
                   .Returns(athletes.ElementType);
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.GetEnumerator())
                   .Returns(athletes.GetEnumerator());
            // Session
            mockSetSession = new Mock<DbSet<Session>>();
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.Provider)
                   .Returns(sessions.Provider);
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.Expression)
                   .Returns(sessions.Expression);
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.ElementType)
                   .Returns(sessions.ElementType);
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.GetEnumerator())
                   .Returns(sessions.GetEnumerator());
            // Session
            mockSetLocation = new Mock<DbSet<Location>>();
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.Provider)
                   .Returns(locations.Provider);
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.Expression)
                   .Returns(locations.Expression);
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.ElementType)
                   .Returns(locations.ElementType);
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.GetEnumerator())
                   .Returns(locations.GetEnumerator());
            // SessionAthlete
            mockSetSessionAthlete = new Mock<DbSet<SessionAthlete>>();
            mockSetSessionAthlete.As<IQueryable<SessionAthlete>>().Setup(m => m.Provider)
                   .Returns(allSessionAthletes.Provider);
            mockSetSessionAthlete.As<IQueryable<SessionAthlete>>().Setup(m => m.Expression)
                   .Returns(allSessionAthletes.Expression);
            mockSetSessionAthlete.As<IQueryable<SessionAthlete>>().Setup(m => m.ElementType)
                   .Returns(allSessionAthletes.ElementType);
            mockSetSessionAthlete.As<IQueryable<SessionAthlete>>().Setup(m => m.GetEnumerator())
                   .Returns(allSessionAthletes.GetEnumerator());
            // Schedule
            mockSetSchedule = new Mock<DbSet<Schedule>>();
            mockSetSchedule.As<IQueryable<Schedule>>().Setup(m => m.Provider)
                   .Returns(schedules.Provider);
            mockSetSchedule.As<IQueryable<Schedule>>().Setup(m => m.Expression)
                   .Returns(schedules.Expression);
            mockSetSchedule.As<IQueryable<Schedule>>().Setup(m => m.ElementType)
                   .Returns(schedules.ElementType);
            mockSetSchedule.As<IQueryable<Schedule>>().Setup(m => m.GetEnumerator())
                   .Returns(schedules.GetEnumerator());

            mockContext = new Mock<IdentityDb>();
            mockContext.Setup(c => c.Athletes).Returns(mockSetAthlete.Object);
            mockContext.Setup(c => c.Sessions).Returns(mockSetSession.Object);
            mockContext.Setup(c => c.Locations).Returns(mockSetLocation.Object);
            mockContext.Setup(c => c.Schedules).Returns(mockSetSchedule.Object);
            return new AthleteRepository(mockContext.Object);
        }

        private IQueryable<Session> MakeListOne(DateTime aug18, Location dardenne)
        {
            aug18_6 = new Session
            {
                Id = 1,
                Hour = 6,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_7 = new Session
            {
                Id = 2,
                Hour = 7,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_8 = new Session
            {
                Id = 3,
                Hour = 8,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_9 = new Session
            {
                Id = 4,
                Hour = 9,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_10 = new Session
            {
                Id = 5,
                Hour = 10,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_16 = new Session
            {
                Id = 6,
                Hour = 16,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_17 = new Session
            {
                Id = 7,
                Hour = 17,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_18 = new Session
            {
                Id = 8,
                Hour = 18,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug18_19 = new Session
            {
                Id = 9,
                Hour = 19,
                Day = aug18,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };

            IList<Session> myList = new List<Session> { aug18_6, aug18_7, aug18_8, aug18_9, aug18_10, aug18_16, aug18_17, aug18_18, aug18_19 };
            return myList.AsQueryable();
        }

        private IQueryable<Session> MakeListTwo(DateTime aug19, Location dardenne)
        {
            aug19_6 = new Session
            {
                Id = 1,
                Hour = 6,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_7 = new Session
            {
                Id = 2,
                Hour = 7,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_8 = new Session
            {
                Id = 3,
                Hour = 8,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_9 = new Session
            {
                Id = 4,
                Hour = 9,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_10 = new Session
            {
                Id = 5,
                Hour = 10,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_16 = new Session
            {
                Id = 6,
                Hour = 16,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_17 = new Session
            {
                Id = 7,
                Hour = 17,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_18 = new Session
            {
                Id = 8,
                Hour = 18,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };
            aug19_19 = new Session
            {
                Id = 9,
                Hour = 19,
                Day = aug19,
                LocationId = dardenne.Id,
                AthleteType = AthleteTypes.PersonalTraining
            };

            IList<Session> myList = new List<Session> {aug19_6, aug19_7, aug19_8, aug19_9, aug19_10, aug19_16, aug19_17, aug19_18, aug19_19};
            return myList.AsQueryable();
        }

 
    }
}

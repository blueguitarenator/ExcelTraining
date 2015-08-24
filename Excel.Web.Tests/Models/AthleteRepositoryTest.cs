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

        private Athlete paul;
        private Athlete john;
        private Athlete george;
        private Athlete ringo;

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
        //public void testRemoveAthleteFromSession()
        //{
        //    var theSession = sessions.Where(s => s.Id == 1).FirstOrDefault();
        //    Assert.IsTrue(theSession.Athletes.Contains(paul));
        //    testObject.RemoveAthleteFromSession(1, paul.Id);
        //    Assert.IsFalse(theSession.Athletes.Contains(paul));
        //}

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
            DateTime saveNow = DateTime.Now.Date;
            var athletes = new List<Athlete> 
            { 
                new Athlete { Id = 1, FirstName = "Kenny", LastName = "Ball", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = saveNow }, 
                new Athlete { Id = 2, FirstName = "Rich", LastName = "Schwepker", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = saveNow }, 
                new Athlete { Id = 3, FirstName = "Erin", LastName = "Sitz", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Trainer, Location = dardenne, SelectedDate = saveNow }, 
                new Athlete { Id = 111, FirstName = "Paul", LastName = "McCartney", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = saveNow }, 
                new Athlete { Id = 222, FirstName = "John", LastName = "Lennon", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = saveNow }, 
                new Athlete { Id = 333, FirstName = "George", LastName = "Harrison", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = saveNow }, 
                new Athlete { Id = 444, FirstName = "Ringo", LastName = "Starr", Address = "444 Primrose", City = "Dardenne", State = "MO", Zip = "63368", UserType = UserTypes.Athlete, Location = dardenne, SelectedDate = saveNow }, 
            }.AsQueryable();
            paul = athletes.ElementAt(3);
            john = athletes.ElementAt(4);
            george = athletes.ElementAt(5);
            ringo = athletes.ElementAt(6);

            var session6Athletes = new List<Athlete> { paul, john, george};
            var session7Athletes = new List<Athlete> { paul, john, george};
            var session8Athletes = new List<Athlete> { paul, john, george};

            //sessions = new List<Session>
            //{
            //    new Session{ Id = 1, Hour =6, Day =saveNow, Athletes=session6Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{ Id = 2, Hour =7, Day =saveNow, Athletes=session7Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{ Id = 3, Hour =8, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.PersonalTraining},
            //    new Session{ Id = 4, Hour =16, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 5, Hour =17, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 6, Hour =18, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 7, Hour =19, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //    new Session{ Id = 8, Hour =20, Day =saveNow, Athletes=session8Athletes, LocationId = dardenne.Id, AthleteType = AthleteTypes.SportsTraining},
            //}.AsQueryable();

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
            var mockSetLocation = new Mock<DbSet<Location>>();
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.Provider)
                   .Returns(locations.Provider);
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.Expression)
                   .Returns(locations.Expression);
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.ElementType)
                   .Returns(locations.ElementType);
            mockSetLocation.As<IQueryable<Location>>().Setup(m => m.GetEnumerator())
                   .Returns(locations.GetEnumerator());
            // Schedule
            var mockSetSchedule = new Mock<DbSet<Schedule>>();
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
    }
}

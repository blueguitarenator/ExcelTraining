using Excel.Entities;
using Excel.Web.Controllers;
using Excel.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Excel.Web.Tests.Controllers
{
    [TestClass]
    public class SessionControllerTest
    {
        //[TestMethod]
        //public void Index_ReturnsSessionModelWithDateAndLocation()
        //{
        //    var saveNow = DateTime.Now.Date;
        //    var athlete = new Athlete() { 
        //        Id = 1, 
        //        FirstName = "Rich", 
        //        LastName = "Johnson", 
        //        SelectedDate = saveNow, 
        //        SelectedLocationId = 1 
        //    };
        //    var athletes = new List<Athlete>(){athlete};
        //    var sessions = CreateSessions(saveNow, athletes);
        //    var dardenne = new Location { Id = 1, Name = "Dardenne Prairie" };
        //    var midrivers = new Location { Id = 2, Name = "Mid Rivers"};
        //    var locations = new List<Location>(){dardenne, midrivers};
          
        //    var mockRepo = new Mock<IAthleteRepository>();
        //    var controller = CreateSessionController(mockRepo.Object);
        //    mockRepo.Setup(x => x.GetAthleteByUserId("1")).Returns(athlete);
        //    mockRepo.Setup(x => x.GetLocations()).Returns(locations);
        //    mockRepo.Setup(x => x.GetSession(6, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[0]);
        //    mockRepo.Setup(x => x.GetSession(7, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[1]);
        //    mockRepo.Setup(x => x.GetSession(8, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[2]);
        //    mockRepo.Setup(x => x.GetSession(9, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[3]);
        //    mockRepo.Setup(x => x.GetSession(10, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[4]);
        //    mockRepo.Setup(x => x.GetSession(16, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[5]);
        //    mockRepo.Setup(x => x.GetSession(17, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[6]);
        //    mockRepo.Setup(x => x.GetSession(18, saveNow, 1, AthleteTypes.PersonalTraining)).Returns(sessions[7]);
        //    mockRepo.Setup(x => x.GetPersonalTrainingAthletes(1, 1)).Returns(athletes);

        //    var result = controller.Index() as ViewResult;

        //    var model = result.ViewData.Model as SessionModel;
        //    Assert.IsInstanceOfType(result.ViewData.Model, typeof(SessionModel));
        //    Assert.AreEqual(saveNow, model.SessionDateTime);
        //    Assert.AreEqual(1, model.SelectedLocationId);
        //    Assert.AreEqual(2, model.LocationSelectList.Count());
        //    //Assert.AreEqual(16, model.SixAmPersonalTraining.Count());
        //    //Assert.AreEqual("Rich", model.SixAmPersonalTraining[0].FirstName);
        //}

        //private List<Session> CreateSessions(DateTime saveNow, List<Athlete> athletes)
        //{
        //    var sessions = new List<Session>() {
        //        new Session(){Id = 1, Hour = 6, Day = saveNow, Athletes = athletes},
        //        new Session(){Id = 2, Hour = 7, Day = saveNow, Athletes = athletes},
        //        new Session(){Id = 3, Hour = 8, Day = saveNow, Athletes = athletes},
        //        new Session(){Id = 4, Hour = 9, Day = saveNow, Athletes = athletes},
        //        new Session(){Id = 5, Hour = 10, Day = saveNow, Athletes = athletes},
        //        new Session(){Id = 6, Hour = 16, Day = saveNow, Athletes = athletes},
        //        new Session(){Id = 7, Hour = 17, Day = saveNow, Athletes = athletes},
        //        new Session(){Id = 8, Hour = 18, Day = saveNow, Athletes = athletes}
        //    };
        //    return sessions;
        //}

        private List<Athlete> GetGridAthletes()
        {
            var sixAm = new List<Athlete>() 
            {
                new Athlete(){FirstName = "Rich", LastName = "Johnson"},
                new Athlete(){FirstName = "Paul", LastName = "McCartney"}
            };
            return sixAm;
        }

        SessionController CreateSessionController(IAthleteRepository repo)
        {
            return new SessionController(repo)
            {
                GetUserId = () => "1"
            };
        }

    }
}

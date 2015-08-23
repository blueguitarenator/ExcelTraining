using Excel.Entities;
using Excel.Web.Controllers;
using Excel.Web.Models;
using Excel.Web.Tests.Models;
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
    public class DashboardControllerTest
    {

        [TestMethod]
        public void Index_LoadsFutureSessionsAndTotalSessions()
        {
            var sessions = new List<Session>();
            var athlete = new Athlete() { Id = 1, FirstName = "Rich", LastName = "Johnson" };
            List<Athlete> myList = new List<Athlete>();
            var mockRepo = new Mock<IAthleteRepository>();
            var controller = CreateDashboardController(mockRepo.Object);
            mockRepo.Setup(x => x.GetAthleteByUserId("1")).Returns(athlete);
            mockRepo.Setup(x => x.GetFutureSessions(1)).Returns(sessions);

            var result = controller.Index() as ViewResult;

            Assert.IsInstanceOfType(result.ViewData.Model, typeof(DashboardModel));
        }

        //[TestMethod]
        //public void RemoveFromSession_UsesRepoCorrectly_AndRedirectsToIndex()
        //{
        //    var athlete = new Athlete() { Id = 1, FirstName = "Rich", LastName = "Johnson" };
        //    List<Athlete> athletes = new List<Athlete>();
        //    athletes.Add(athlete);
        //    var session = new Session() { Id = 1, Hour = 6, LocationId = 1, Athletes = athletes };
        //    var mockRepo = new Mock<IAthleteRepository>();
        //    var controller = CreateDashboardController(mockRepo.Object);
        //    mockRepo.Setup(x => x.GetAthleteByUserId("1")).Returns(athlete);
        //    mockRepo.Setup(x => x.GetSessionById(1)).Returns(session);

        //    var result = controller.RemoveFromSession(session.Id) as RedirectToRouteResult;

        //    Assert.AreEqual("Index", result.RouteValues["Action"]);
        //    mockRepo.Verify(x => x.RemoveAthleteFromSession(session.Id, athlete.Id));
        //    mockRepo.Verify(x => x.SaveChanges());
        //}

        DashboardController CreateDashboardController(IAthleteRepository repo)
        {
            //var repository = new InMemoryAthleteRepository(CreateTestAthletes());
            return new DashboardController(repo)
            {
                GetUserId = () => "1"
            };
        }

        List<Athlete> CreateTestAthletes()
        {
            List<Athlete> athletes = new List<Athlete>();

            for (int i = 0; i < 101; i++)
            {

                Athlete sampleAthlete = new Athlete()
                {
                    Id = i,
                    FirstName = "SamplefirstName",
                    LastName = "SomeLastName",
                    Address = "Some Address",
                    City = "SomeCity",
                    State = "MO",
                    Zip = "63366",
                    AthleteType = AthleteTypes.PersonalTraining,
                    UserType = UserTypes.Athlete,
                    LocationId = 1
                };

                athletes.Add(sampleAthlete);
            }
            return athletes;
        }

        // NOT USED
        DashboardController CreateDashboardControllerAs(string userName)
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(userName);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var mockRepo = new Mock<IAthleteRepository>();
            var controller = CreateDashboardController(mockRepo.Object);
            controller.ControllerContext = mock.Object;

            return controller;
        }
    }
}

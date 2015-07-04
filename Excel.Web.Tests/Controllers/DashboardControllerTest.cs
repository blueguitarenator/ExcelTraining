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
        public void IndexAction_ShouldReturnViewFor_DashboardModel()
        {

            // Arrange
            var controller = CreateDashboardController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(DashboardModel));
        }

        DashboardController CreateDashboardControllerAs(string userName)
        {

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(userName);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var controller = CreateDashboardController();
            controller.ControllerContext = mock.Object;

            return controller;
        }

        DashboardController CreateDashboardController()
        {
            var repository = new InMemoryAthleteRepository(CreateTestAthletes());
            return new DashboardController(repository)
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
    }
}

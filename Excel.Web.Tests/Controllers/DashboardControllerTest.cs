using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excel.Web.Controllers;
using Excel.Web.Models;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Security.Principal;
using Excel.Web.Tests.Models;
using Excel.Entities;

namespace Excel.Web.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTest
    {
        private static DashboardController GetDashboardController(IAthleteRepository repository)
        {
            DashboardController controller = new DashboardController(repository);

            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return controller;
        }


        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(
                     new GenericIdentity("someUser"), null /* roles */);

            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }

        [TestMethod]
        public void Index_Get_AsksForIndexView()
        {
            // Arrange
            var inMemoryAthleteRepository = new InMemoryAthleteRepository();
            inMemoryAthleteRepository.CreateNewAthlete(new Athlete{ Id = 1, FirstName = "Rich", LastName = "Johnson"});
            var controller = GetDashboardController(inMemoryAthleteRepository);
            // Act
            var result = controller.Index() as ViewResult;
            // Assert
            DashboardModel model = result.Model as DashboardModel;
            Assert.AreEqual(1, model.MySessions.Count);
        } 
    }
}

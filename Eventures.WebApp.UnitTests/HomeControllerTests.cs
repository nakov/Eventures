using System.Linq;

using Eventures.Tests.Common;
using Eventures.WebApp.Models;
using Eventures.WebApp.Controllers;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Eventures.WebApp.UnitTests
{
    public class HomeControllerTests : UnitTestsBase
    {
        private HomeController controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Instantiate the controller class with the db context
            this.controller = new HomeController(this.dbContext);
        }

        [Test]
        public void Test_Index()
        {
            // Arrange: assign UserMaria to the controller
            TestingUtils.AssignCurrentUserForController(
                this.controller, this.testDb.UserMaria);

            // Act: invoke the controller method
            var result = this.controller.Index();

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert the returned model is correct
            var resultModel = viewResult.Model as HomeViewModel;
            Assert.AreEqual(this.dbContext.Events.Count(),
                resultModel.AllEventsCount);
        }

        [Test]
        public void Test_Error()
        {
            // Arrange

            // Act: invoke the controller method
            var result = this.controller.Error();

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void Test_Error401()
        {
            // Arrange

            // Act: invoke the controller method
            var result = this.controller.Error401();

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void Test_Error404()
        {
            // Arrange

            // Act: invoke the controller method
            var result = this.controller.Error404();

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}

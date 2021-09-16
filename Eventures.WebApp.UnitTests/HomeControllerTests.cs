using System.Linq;

using Eventures.WebApp.Controllers;
using Eventures.Tests.Common;
using Eventures.WebApp.Models;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Eventures.WebApp.UnitTests
{
    public class HomeControllerTests : UnitTestsBase
    {
        [Test]
        public void Test_Index()
        {
            // Arrange
            var controller = new HomeController(this.dbContext);
            TestingUtils
                .AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var resultModel = viewResult.Model as HomeViewModel;
            Assert.AreEqual(this.dbContext.Events.Count(), resultModel.AllEventsCount);
        }

        [Test]
        public void Test_Error()
        {
            // Arrange
            var controller = new HomeController(this.dbContext);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            TestingUtils
                .AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void Test_Error401()
        {
            // Arrange
            var controller = new HomeController(this.dbContext);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            TestingUtils
                .AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = controller.Error401();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}

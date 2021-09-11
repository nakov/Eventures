using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Eventures.WebApp.Models;
using Eventures.WebApp.Controllers;
using Eventures.Tests.Common;

namespace Eventures.WebApp.UnitTests
{
    public class HomeControllerTests : UnitTestsBase
    {
        [Test]
        public void Test_Index()
        {
            // Arrange
            var controller = new HomeController(dbContext);
            TestingUtils
                .AssignCurrentUserForController(controller, testDb.UserMaria);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void Test_Error()
        {
            // Arrange
            var controller = new HomeController(dbContext);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            TestingUtils
                .AssignCurrentUserForController(controller, testDb.UserMaria);

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
            var controller = new HomeController(dbContext);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            TestingUtils
                .AssignCurrentUserForController(controller, testDb.UserMaria);

            // Act
            var result = controller.Error401();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}

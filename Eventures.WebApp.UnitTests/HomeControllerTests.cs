using System.Linq;

using Eventures.WebApp.Models;
using Eventures.WebApp.Controllers;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Eventures.Tests.Common;

namespace Eventures.WebApp.UnitTests
{
    public class HomeControllerTests : UnitTestsBase
    {
        private HomeController controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Instantiate the controller class with the testing database
            this.controller = new HomeController(
                this.testDb.CreateDbContext());
        }

        [Test]
        public void Test_Index()
        {
            // Arrange: assign UserMaria to the controller
            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = this.controller.Index();

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
        
            // Act
            var result = this.controller.Error();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void Test_Error401()
        {
            // Arrange
         
            // Act
            var result = this.controller.Error401();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void Test_Error404()
        {
            // Arrange

            // Act
            var result = this.controller.Error404();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}

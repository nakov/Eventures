using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using NUnit.Framework;

using Eventures.App.Controllers;
using Eventures.App.Models;
using Eventures.App.Data;

namespace Eventures.UnitTests
{
    public class HomeControllerTests
    {
        TestDb testDb;
        ApplicationDbContext dbContext;

        [OneTimeSetUp]
        public void Setup()
        {
            testDb = new TestDb();
            dbContext = testDb.CreateDbContext();
        }

        [Test]
        public void Test_Index()
        {
            // Arrange
            var controller = new HomeController(dbContext);

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

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as ErrorViewModel;
            Assert.IsNotNull(model);
        }
    }
}

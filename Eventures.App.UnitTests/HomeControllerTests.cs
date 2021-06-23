using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Eventures.App.Models;
using Eventures.App.Controllers;

namespace Eventures.App.UnitTests
{
    public class HomeControllerTests : UnitTestsBase
    {
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

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Eventures.WebApp.Models;
using Eventures.WebApp.Controllers;

namespace Eventures.WebApp.UnitTests
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
            var resultModel = viewResult.Model as ErrorViewModel;
            Assert.IsNotNull(resultModel);
        }
    }
}

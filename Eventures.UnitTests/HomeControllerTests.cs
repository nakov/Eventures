using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

using Eventures.App.Controllers;

namespace Eventures.UnitTests
{
    public class HomeControllerTests
    {
        [Test]
        public void TestHomeControllerGetCreate()
        {
            // Arrange
            var controller = new EventsController(new TestData().DbContext);

            // Act
            var result = controller.Create();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}

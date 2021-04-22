using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

using Eventures.App.Controllers;
using Eventures.App.Models;

namespace Eventures.UnitTests
{
    public class EventControllerTests
    {
        [Test]
        public void TestEventsControllerGetAll()
        {
            // Arrange
            var testData = new TestData();
            var controller = new EventsController(testData.DbContext);

            // Act
            var result = controller.All();

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<EventViewModel>;

            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("Softuniada 2021", model[0].Name);
            Assert.AreEqual("OpenFest 2021", model[1].Name);
        }

        [Test]
        public void TestEventsControllerGetCreate()
        {
            // Arrange
            var testData = new TestData();
            var controller = new EventsController(testData.DbContext);

            // Act
            var result = controller.Create();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void TestEventsControllerPostCreate()
        {
            // Arrange
            var testData = new TestData();
            var controller = new EventsController(testData.DbContext);
            TestData.AssignCurrentUserForController(controller, testData.UserMaria);
            var newEventData = new EventCreateBindingModel()
            {
                Name = "New Event",
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };

            // Act
            var result = controller.Create(newEventData);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);

            var newEventFromDb =
                testData.DbContext.Events.FirstOrDefault(e => e.Name == newEventData.Name);
            Assert.AreEqual(newEventData.Place, newEventFromDb.Place);
            Assert.IsTrue(newEventFromDb.Id > 0);
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

using Eventures.App.Controllers;
using Eventures.App.Models;

namespace Eventures.UnitTests
{




    // TODO: check why unit tests are so slow!
    // Slower than the integration tests




    public class EventControllerTests
    {
        [Test]
        public void Test_All()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);

            // Act
            var result = controller.All();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as List<EventViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual(testDb.EventSoftuniada.Name, model[0].Name);
            Assert.AreEqual(testDb.EventOpenFest.Name, model[1].Name);
        }

        [Test]
        public void Test_Create()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);

            // Act
            var result = controller.Create();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as EventCreateBindingModel;
            Assert.IsNotNull(model);
        }

        [Test]
        public void Test_Create_PostValidData()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);
            var newEventData = new EventCreateBindingModel()
            {
                Name = "New Event " + DateTime.Now.Ticks,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };
            int eventsCountBefore = dbContext.Events.Count();
            TestDb.AssignCurrentUserForController(controller, testDb.UserMaria);

            // Act
            var result = controller.Create(newEventData);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);

            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountAfter == eventsCountBefore + 1);

            var newEventFromDb =
                dbContext.Events.FirstOrDefault(e => e.Name == newEventData.Name);
            Assert.IsTrue(newEventFromDb.Id > 0);
            Assert.AreEqual(newEventData.Place, newEventFromDb.Place);
            Assert.AreEqual(newEventData.Start, newEventFromDb.Start);
            Assert.AreEqual(newEventData.End, newEventFromDb.End);
            Assert.AreEqual(newEventData.PricePerTicket, newEventFromDb.PricePerTicket);
            Assert.AreEqual(newEventData.TotalTickets, newEventFromDb.TotalTickets);
        }

        [Test]
        public void Test_Create_PostInvalidData()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);
            var newEventData = new EventCreateBindingModel()
            {
                Name = null,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };

            int eventsCountBefore = dbContext.Events.Count();

            TestDb.AssignCurrentUserForController(controller, testDb.UserMaria);
            controller.ModelState.AddModelError("Name", "The Name field is required");

            //// Act
            var result = controller.Create(newEventData);

            //// Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountAfter == eventsCountBefore);
        }

        [Test]
        public void Test_Delete_ValidId()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);

            // Act
            var result = controller.Delete(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as EventViewModel;
            Assert.IsNotNull(model);
            Assert.That(model.Id == 1);
            Assert.That(model.Name == testDb.EventSoftuniada.Name);
            Assert.That(model.Place == testDb.EventSoftuniada.Place);
        }

        [Test]
        public void Test_Delete_InvalidId()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);

            // Act
            var result = controller.Delete(-1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as EventViewModel;
            Assert.IsNull(model);
        }

        [Test]
        public void Test_Delete_PostValidData()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);
            EventViewModel model = new EventViewModel()
            {
                Id = 1
            };
            int eventsCountBefore = dbContext.Events.Count();

            // Act
            var result = controller.Delete(model);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);
            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountAfter == eventsCountBefore - 1);
            var deletedEvent = dbContext.Events.Find(model.Id);
            Assert.IsNull(deletedEvent);
        }

        [Test]
        public void Test_Delete_PostInvalidData()
        {
            // Arrange
            var testDb = new TestDb();
            var dbContext = testDb.CreateDbContext();
            var controller = new EventsController(dbContext);
            EventViewModel model = new EventViewModel()
            {
                Id = -1
            };
            int eventsCountBefore = dbContext.Events.Count();

            // Act
            var result = controller.Delete(model);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var resultModel = viewResult.Model as EventViewModel;
            Assert.IsNull(resultModel);
            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountAfter == eventsCountBefore);
        }
    }
}

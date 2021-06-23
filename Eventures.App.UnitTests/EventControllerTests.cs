using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

using Eventures.Data;
using Eventures.App.Models;
using Eventures.App.Controllers;
using Eventures.Tests.Common;

namespace Eventures.App.UnitTests
{
    public class EventControllerTests : UnitTestsBase
    {
        private EventsController controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.controller = new EventsController(this.testDb.CreateDbContext());
        }

        [Test]
        public void Test_All()
        {
            // Arrange
            
            // Act
            var result = controller.All();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as List<EventViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
            Assert.AreEqual(testDb.EventSoftuniada.Name, model[0].Name);
            Assert.AreEqual(testDb.EventOpenFest.Name, model[1].Name);
        }

        [Test]
        public void Test_Create()
        {
            // Arrange

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
            TestingUtils.AssignCurrentUserForController(controller, testDb.UserMaria);

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
            string invalidName = null;
            var newEventData = new EventCreateBindingModel()
            {
                Name = invalidName,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };

            int eventsCountBefore = dbContext.Events.Count();

            TestingUtils.AssignCurrentUserForController(controller, testDb.UserMaria);
            controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act
            var result = controller.Create(newEventData);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountAfter == eventsCountBefore);

            // Remove ModelState error for next tests
            controller.ModelState.Remove("Name");
        }

        [Test]
        public void Test_DeletePage_ValidId()
        {
            // Arrange: create a new event in the DB for deleting
            TestingUtils.AssignCurrentUserForController(controller, testDb.UserMaria);
            var newEvent = new Event()
            {
                Name = "Beach Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m,
                OwnerId = testDb.UserMaria.Id
            };
            dbContext.Add(newEvent);
            dbContext.SaveChanges();

            // Act
            var result = controller.Delete(newEvent.Id);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as EventViewModel;
            Assert.IsNotNull(model);
            Assert.That(model.Id == newEvent.Id);
            Assert.That(model.Name == newEvent.Name);
            Assert.That(model.Place == newEvent.Place);
        }

        [Test]
        public void Test_DeletePage_InvalidId()
        {
            // Arrange
            TestingUtils.AssignCurrentUserForController(controller, testDb.UserMaria);

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
            // Arrange: create a new event in the DB for deleting
            var newEvent = new Event()
            {
                Name = "Beach Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m,
                OwnerId = testDb.UserMaria.Id
            };
            dbContext.Add(newEvent);
            dbContext.SaveChanges();

            EventViewModel model = new EventViewModel()
            {
                Id = newEvent.Id
            };

            int eventsCountBefore = dbContext.Events.Count();

            // Act
            var result = controller.Delete(model);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);
            dbContext = this.testDb.CreateDbContext();
            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountAfter == eventsCountBefore - 1);
            var deletedEvent = dbContext.Events.Find(model.Id);
            Assert.IsNull(deletedEvent);
        }

        [Test]
        public void Test_Delete_PostInvalidData()
        {
            // Arrange
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

        [Test]
        public void Test_Edit_ValidId()
        {
            // Arrange: get the "Softuniada" event from the database for editing
            var softuniadaEvent = this.testDb.EventSoftuniada;
            TestingUtils.AssignCurrentUserForController(controller, testDb.UserMaria);

            // Act
            var result = controller.Edit(softuniadaEvent.Id);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert fields are filled with correct data
            var model = viewResult.Model as EventCreateBindingModel;
            Assert.IsNotNull(model);
            Assert.That(model.Name == softuniadaEvent.Name);
            Assert.That(model.Place == softuniadaEvent.Place);
            Assert.That(model.Start == softuniadaEvent.Start);
            Assert.That(model.End == softuniadaEvent.End);
            Assert.That(model.PricePerTicket == softuniadaEvent.PricePerTicket);
            Assert.That(model.TotalTickets == softuniadaEvent.TotalTickets);
        }

        [Test]
        public void Test_Edit_InvalidId()
        {
            // Arrange
            TestingUtils.AssignCurrentUserForController(controller, testDb.UserMaria);

            // Act
            var result = controller.Edit(-1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as EventCreateBindingModel;
            Assert.IsNull(model);
        }

        [Test]
        public void Test_Edit_PostValidData()
        {
            // Arrange: create a new event in the database for editing
            var newEvent = new Event()
            {
                Name = "Beach Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m,
                OwnerId = testDb.UserMaria.Id
            };
            dbContext.Add(newEvent);
            dbContext.SaveChanges();

            // Only the name is changed
            var changedName = "Party" + DateTime.Now.Ticks;
            EventCreateBindingModel model = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m
            };

            // Act
            var result = controller.Edit(newEvent.Id, model);

            // Assert redirection to "All Events" page
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);

            // Assert that name of the event is changed in the database
            dbContext = this.testDb.CreateDbContext();
            var editedEvent = dbContext.Events.Find(newEvent.Id);
            Assert.IsNotNull(editedEvent);
            Assert.AreEqual(model.Name, editedEvent.Name);
        }

        [Test]
        public void Test_Edit_PostInvalidData()
        {
            // Arrange: get the "Softuniada" event from the database for editing
            var softuniadaEvent = this.testDb.EventSoftuniada;
            TestingUtils.AssignCurrentUserForController(controller, testDb.UserMaria);

            // The changed name is invalid: name == empty string
            var invalidName = string.Empty;
            EventCreateBindingModel model = new EventCreateBindingModel()
            {
                Name = invalidName,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 200,
                PricePerTicket = 12.00m
            };

            controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act
            var result = controller.Edit(softuniadaEvent.Id, model);

            // Assert user is not redirected
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNull(redirectResult);

            // Assert that event's name is not edited
            var editedEvent = dbContext.Events.Find(softuniadaEvent.Id);
            Assert.IsNotNull(editedEvent);
            Assert.AreEqual(softuniadaEvent.Name, editedEvent.Name);

            // Remove ModelState error for next tests
            controller.ModelState.Remove("Name");
        }
    }
}

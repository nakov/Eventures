using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using Eventures.Data;
using Eventures.WebApp.Models;
using Eventures.WebApp.Controllers;
using Eventures.Tests.Common;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Eventures.WebApp.UnitTests
{
    public class EventControllerTests : UnitTestsBase
    {
        private EventsController controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Instantiate the controller class with the testing database
            this.controller = new EventsController(this.dbContext);

            // Set UserMaria as current logged user
            TestingUtils.AssignCurrentUserForController(this.controller, this.testDb.UserMaria);
        }

        [Test]
        public void Test_All()
        {
            // Arrange

            // Act: invoke the controller method
            var result = this.controller.All();

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert events count is correct
            var resultModel = viewResult.Model as List<EventViewModel>;
            Assert.IsNotNull(resultModel);
            Assert.AreEqual(this.dbContext.Events.Count(), resultModel.Count);

            // Assert events are correct
            Assert.AreEqual(this.testDb.EventSoftuniada.Name, resultModel[0].Name);
            Assert.AreEqual(this.testDb.EventOpenFest.Name, resultModel[1].Name);
            Assert.AreEqual(this.testDb.EventMSBuild.Name, resultModel[2].Name);
            Assert.AreEqual(this.testDb.EventDevConf.Name, resultModel[3].Name);
        }

        [Test]
        public void Test_Create()
        {
            // Arrange

            // Act: invoke the controller method
            var result = this.controller.Create();

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert an event model is returned
            var resultModel = viewResult.Model as EventCreateBindingModel;
            Assert.IsNotNull(resultModel);
        }

        [Test]
        public void Test_Create_PostValidData()
        {
            // Arrange: get the events count before the creation
            int eventsCountBefore = this.dbContext.Events.Count();

            // Create an event binding model
            var newEventData = new EventCreateBindingModel()
            {
                Name = "New Event " + DateTime.Now.Ticks,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };

            // Act: invoke the controller method
            var result = this.controller.Create(newEventData);

            // Assert the user is redirected to the "All Events" page
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);

            // Assert the count of events is +1
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore + 1, eventsCountAfter);

            // Assert the new event appeared in the database
            var newEventInDb =
                this.dbContext.Events.FirstOrDefault(e => e.Name == newEventData.Name);
            Assert.IsTrue(newEventInDb.Id > 0);
            Assert.AreEqual(newEventData.Place, newEventInDb.Place);
            Assert.AreEqual(newEventData.Start, newEventInDb.Start);
            Assert.AreEqual(newEventData.End, newEventInDb.End);
            Assert.AreEqual(newEventData.PricePerTicket, newEventInDb.PricePerTicket);
            Assert.AreEqual(newEventData.TotalTickets, newEventInDb.TotalTickets);
        }

        [Test]
        public void Test_Create_PostInvalidData()
        {
            // Arrange: get the events count before the creation
            int eventsCountBefore = this.dbContext.Events.Count();

            // Create create an event binding model with invalid name: name == empty string
            string invalidName = string.Empty;
            var newEventData = new EventCreateBindingModel()
            {
                Name = invalidName,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };

            // Add error to the controller for the invalid name
            this.controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act: invoke the controller method
            var result = this.controller.Create(newEventData);

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert the new event is not created
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);

            // Remove ModelState error for next tests
            this.controller.ModelState.Remove("Name");
        }

        [Test]
        public void Test_DeletePage_ValidId()
        {
            // Arrange: create a new event in the database for deleting
            var newEvent = new Event()
            {
                Name = "Beach Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m,
                OwnerId = this.testDb.UserMaria.Id
            };
            this.dbContext.Add(newEvent);
            this.dbContext.SaveChanges();

            // Act: invoke the controller method
            var result = this.controller.Delete(newEvent.Id);

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert the event was deleted and returned
            var resultModel = viewResult.Model as EventViewModel;
            Assert.IsNotNull(resultModel);
            Assert.That(resultModel.Id == newEvent.Id);
            Assert.That(resultModel.Name == newEvent.Name);
            Assert.That(resultModel.Place == newEvent.Place);
        }

        [Test]
        public void Test_DeletePage_InvalidId()
        {
            // Arrange: get the events count before the deletion
            int eventsCountBefore = this.dbContext.Events.Count();

            var invalidId = -1;

            // Act: invoke the controller method with invalid id
            var result = this.controller.Delete(invalidId);

            // Assert a "Bad Request" result is returned
            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);

            // Assert the event was not deleted
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public void Test_Delete_PostValidData()
        {
            // Arrange: create a new event in the database for deleting
            var newEvent = new Event()
            {
                Name = "Beach Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m,
                OwnerId = this.testDb.UserMaria.Id
            };
            this.dbContext.Add(newEvent);
            this.dbContext.SaveChanges();

            // Get the events count before the deletion
            int eventsCountBefore = this.dbContext.Events.Count();

            // Create a view model with the new event id
            EventViewModel model = new EventViewModel()
            {
                Id = newEvent.Id
            };

            // Act: invoke the controller method
            var result = this.controller.Delete(model);

            // Assert a "Redirect" result is returned
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);

            // Assert the event is deleted from the db
            this.dbContext = this.testDb.CreateDbContext();
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore - 1, eventsCountAfter);

            var deletedEventInDb = this.dbContext.Events.Find(model.Id);
            Assert.IsNull(deletedEventInDb);
        }

        [Test]
        public void Test_Delete_PostInvalidData()
        {
            // Arrange: get the events count before the deletion
            int eventsCountBefore = this.dbContext.Events.Count();

            // Create a view model with invalid id
            var invalidId = -1;
            EventViewModel model = new EventViewModel()
            {
                Id = invalidId
            };

            // Act: invoke the controller method
            var result = this.controller.Delete(model);

            // Assert a "Bad Request" result is returned
            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);

            // Assert the event is not deleted from the database
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public void Test_Delete_UnauthorizedUser()
        {
            // Arrange: get the "OpenFest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            // Arrange: get the events count before the deletion
            int eventsCountBefore = this.dbContext.Events.Count();

            //Create a view model with the new event id
            EventViewModel model = new EventViewModel()
            {
                Id = openFestEvent.Id
            };

            // Act: invoke the controller method
            var result = this.controller.Delete(model);

            // Assert an "Unauthorized" result is returned
            // because UserMaria is not the owner of the "OpenFest" event
            var unauthorizedResult = result as UnauthorizedResult;
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, unauthorizedResult.StatusCode);
            Assert.IsNotNull(unauthorizedResult);

            // Assert the event is not deleted from the database
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public void Test_Edit_ValidId()
        {
            // Arrange: get the "Dev Conference" event from the database for editing
            var devConfEvent = this.testDb.EventDevConf;

            // Act: invoke the controller method
            var result = this.controller.Edit(devConfEvent.Id);

            // Assert a view is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert fields are filled with correct data
            var resultModel = viewResult.Model as EventCreateBindingModel;
            Assert.IsNotNull(resultModel);
            Assert.AreEqual(resultModel.Name, devConfEvent.Name);
            Assert.AreEqual(resultModel.Place, devConfEvent.Place);
            Assert.AreEqual(resultModel.Start, devConfEvent.Start);
            Assert.AreEqual(resultModel.End, devConfEvent.End);
            Assert.AreEqual(resultModel.PricePerTicket, devConfEvent.PricePerTicket);
            Assert.AreEqual(resultModel.TotalTickets, devConfEvent.TotalTickets);
        }

        [Test]
        public void Test_Edit_InvalidId()
        {
            // Arrange

            var invalidId = -1;
            // Act: invoke the controller method with invalid id
            var result = this.controller.Edit(invalidId);

            // Assert a "Bad Request" result is returned
            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
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
                OwnerId = this.testDb.UserMaria.Id
            };
            this.dbContext.Add(newEvent);
            this.dbContext.SaveChanges();

            // Create an event binding model where only the name is changed
            var changedName = "Party" + DateTime.Now.Ticks;
            EventCreateBindingModel model = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = newEvent.Place,
                Start = newEvent.Start,
                End = newEvent.End,
                TotalTickets = newEvent.TotalTickets,
                PricePerTicket = newEvent.PricePerTicket
            };

            // Act: invoke the controller method
            var result = this.controller.Edit(newEvent.Id, model);

            // Assert a "Redirect" result is returned
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);

            // Assert the name of the event is edited in the database
            this.dbContext = this.testDb.CreateDbContext();
            var editedEvent = this.dbContext.Events.Find(newEvent.Id);
            Assert.IsNotNull(editedEvent);
            Assert.AreEqual(model.Name, editedEvent.Name);
        }

        [Test]
        public void Test_Edit_PostInvalidData()
        {
            // Arrange: get the "Dev Conference" event from the database for editing
            var devConfEvent = this.testDb.EventDevConf;

            // Create an event binding model with invalid name: name == empty string
            var invalidName = string.Empty;
            EventCreateBindingModel model = new EventCreateBindingModel()
            {
                Name = invalidName,
                Place = devConfEvent.Place,
                Start = devConfEvent.Start,
                End = devConfEvent.End,
                TotalTickets = devConfEvent.TotalTickets,
                PricePerTicket = devConfEvent.PricePerTicket
            };

            // Add error to the controller
            this.controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act: invoke the controller method
            var result = this.controller.Edit(devConfEvent.Id, model);

            // Assert the user is not redirected
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNull(redirectResult);

            // Assert the event's name is not edited
            var editedEvent = this.dbContext.Events.Find(devConfEvent.Id);
            Assert.IsNotNull(editedEvent);
            Assert.AreEqual(devConfEvent.Name, editedEvent.Name);

            // Remove ModelState error for next tests
            this.controller.ModelState.Remove("Name");
        }

        [Test]
        public void Test_Edit_UnauthorizedUser()
        {
            // Arrange: get the "Open Fest" event with owner UserPeter
            var openFestEvent = this.testDb.EventOpenFest;

            // Create event binding model with changed event name
            var changedName = "OpenFest 2021 (New Edition)";
            var changedEvent = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = openFestEvent.Place,
                Start = openFestEvent.Start,
                End = openFestEvent.End,
                TotalTickets = openFestEvent.TotalTickets,
                PricePerTicket = openFestEvent.PricePerTicket
            };

            // Act: invoke the controller method
            var result = this.controller.Edit(openFestEvent.Id, changedEvent);

            // Assert an "Unauthorized" result is returned
            // because UserMaria is not the owner of the "Open Fest" event
            var unauthorizedResult = result as UnauthorizedResult;
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, unauthorizedResult.StatusCode);
            Assert.IsNotNull(unauthorizedResult);

            // Assert the event's name is not edited
            var editedEvent = this.dbContext.Events.Find(openFestEvent.Id);
            Assert.IsNotNull(editedEvent);
            Assert.AreEqual(openFestEvent.Name, editedEvent.Name);
        }
    }
}

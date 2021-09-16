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
            this.controller = new EventsController(
                this.testDb.CreateDbContext());
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

            // Assert events count is correct
            var resultModel = viewResult.Model as List<EventViewModel>;
            Assert.IsNotNull(resultModel);
            Assert.AreEqual(3, resultModel.Count);

            // Assert events are correct
            Assert.AreEqual(this.testDb.EventSoftuniada.Name, resultModel[0].Name);
            Assert.AreEqual(this.testDb.EventOpenFest.Name, resultModel[1].Name);
            Assert.AreEqual(this.testDb.EventMSBuild.Name, resultModel[2].Name);
        }

        [Test]
        public void Test_Create()
        {
            // Arrange

            // Act
            var result = controller.Create();

            // Assert an event model is returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var resultModel = viewResult.Model as EventCreateBindingModel;
            Assert.IsNotNull(resultModel);
        }

        [Test]
        public void Test_Create_PostValidData()
        {
            // Arrange: create an event binding model
            var newEventData = new EventCreateBindingModel()
            {
                Name = "New Event " + DateTime.Now.Ticks,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };
            int eventsCountBefore = this.dbContext.Events.Count();

            // Set UserMaria as current logged user
            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = controller.Create(newEventData);

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
            // Arrange: create create an event binding model with invalid name: name == empty string
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

            int eventsCountBefore = this.dbContext.Events.Count();

            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Add error to the controller for the invalid name
            controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act
            var result = controller.Create(newEventData);

            // Assert the new event is not created
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);

            // Remove ModelState error for next tests
            controller.ModelState.Remove("Name");
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

            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = controller.Delete(newEvent.Id);

            // Assert the event was deleted and returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var resultModel = viewResult.Model as EventViewModel;
            Assert.IsNotNull(resultModel);
            Assert.That(resultModel.Id == newEvent.Id);
            Assert.That(resultModel.Name == newEvent.Name);
            Assert.That(resultModel.Place == newEvent.Place);
        }

        [Test]
        public void Test_DeletePage_InvalidId()
        {
            // Arrange
            int eventsCountBefore = this.dbContext.Events.Count();
            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            var invalidId = -1;
            // Act: send invalid id
            var result = controller.Delete(invalidId);

            // Assert the event was not deleted and returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var resultModel = viewResult.Model as EventViewModel;
            Assert.IsNull(resultModel);

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

            // Create a view model with the new event id as content
            EventViewModel model = new EventViewModel()
            {
                Id = newEvent.Id
            };

            int eventsCountBefore = this.dbContext.Events.Count();

            // Act
            var result = controller.Delete(model);

            // Assert the user is redirected to the "All Events" page
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("All", redirectResult.ActionName);

            // Assert the event is deleted
            this.dbContext = this.testDb.CreateDbContext();
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore - 1, eventsCountAfter);

            var deletedEventInDb = this.dbContext.Events.Find(model.Id);
            Assert.IsNull(deletedEventInDb);
        }

        [Test]
        public void Test_Delete_PostInvalidData()
        {
            // Arrange: create a view model with invalid id
            var invalidId = -1;
            EventViewModel model = new EventViewModel()
            {
                Id = invalidId
            };
            int eventsCountBefore = this.dbContext.Events.Count();

            // Act
            var result = controller.Delete(model);

            // Assert the deletion was not successful
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var resultModel = viewResult.Model as EventViewModel;
            Assert.IsNull(resultModel);

            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public void Test_Delete_UnauthorizedUser()
        {
            // Arrange: get the "Open Fest" event with owner UserPeter
            var openFestEvent = this.testDb.EventOpenFest;

            //Create a view model with the new event id as content
            EventViewModel model = new EventViewModel()
            {
                Id = openFestEvent.Id
            };

            // Assign UserMaria to the controller
            int eventsCountBefore = this.dbContext.Events.Count();

            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = controller.Delete(model);

            // Assert an "Unautorized" error message appears 
            // because UserMaria is not the owner of the "Open Fest" event
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
            // Arrange: get the "Softuniada" event from the database for editing
            var softuniadaEvent = this.testDb.EventSoftuniada;
            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Act
            var result = controller.Edit(softuniadaEvent.Id);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            // Assert fields are filled with correct data
            var resultModel = viewResult.Model as EventCreateBindingModel;
            Assert.IsNotNull(resultModel);
            Assert.AreEqual(resultModel.Name, softuniadaEvent.Name);
            Assert.AreEqual(resultModel.Place, softuniadaEvent.Place);
            Assert.AreEqual(resultModel.Start, softuniadaEvent.Start);
            Assert.AreEqual(resultModel.End, softuniadaEvent.End);
            Assert.AreEqual(resultModel.PricePerTicket, softuniadaEvent.PricePerTicket);
            Assert.AreEqual(resultModel.TotalTickets, softuniadaEvent.TotalTickets);
        }

        [Test]
        public void Test_Edit_InvalidId()
        {
            // Arrange
            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            var invalidId = -1;
            // Act: send invalid id
            var result = controller.Edit(invalidId);

            // Assert an event is not returned
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var resultModel = viewResult.Model as EventCreateBindingModel;
            Assert.IsNull(resultModel);
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
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m
            };

            // Act
            var result = controller.Edit(newEvent.Id, model);

            // Assert the user is redirected to the "All Events" page
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
            // Arrange: get the "Softuniada" event from the database for editing
            var softuniadaEvent = this.testDb.EventSoftuniada;
            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Create an event binding model with invalid name: name == empty string
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

            // Add error to the controller
            controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act
            var result = controller.Edit(softuniadaEvent.Id, model);

            // Assert the user is not redirected
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNull(redirectResult);

            // Assert the event's name is not edited
            var editedEvent = this.dbContext.Events.Find(softuniadaEvent.Id);
            Assert.IsNotNull(editedEvent);
            Assert.AreEqual(softuniadaEvent.Name, editedEvent.Name);

            // Remove ModelState error for next tests
            this.controller.ModelState.Remove("Name");
        }

        [Test]
        public void Test_Edit_UnauthorizedUser()
        {
            // Arrange: get the "Open Fest" event with owner UserPeter
            var openFestEvent = this.testDb.EventOpenFest;

            // Assign UserMaria to the controller
            TestingUtils.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Create event binding model with changed event name
            var changedName = "OpenFest 2021 (New Edition)";
            var changedEvent = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = "Online",
                Start = DateTime.Now.AddDays(200),
                End = DateTime.Now.AddDays(201),
                TotalTickets = 5000,
                PricePerTicket = 10.00m,
            };

            // Act
            var result = controller.Edit(openFestEvent.Id, changedEvent);

            // Assert "Unautorized" error message appears 
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

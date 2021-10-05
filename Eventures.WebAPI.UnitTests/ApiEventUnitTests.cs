using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;

using Eventures.Data;
using Eventures.Tests.Common;
using Eventures.WebAPI.Models;
using Eventures.WebAPI.Controllers;
using Eventures.WebAPI.Models.Event;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Eventures.WebAPI.UnitTests
{
    public class ApiEventUnitTests : ApiUnitTestsBase
    {
        EventsController eventsController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.eventsController = new EventsController(this.dbContext);
        }
        
        [Test]
        public void Test_Events_GetCount()
        {
            // Arrange

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.GetEventsCount() as OkObjectResult;
            Assert.IsNotNull(result);

            // Assert an "OK" result with a correct events count is returned
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            var resultValue = (int)result.Value;
            Assert.AreEqual(this.dbContext.Events.Count(), resultValue);
        }

        [Test]
        public void Test_Events_GetEvents()
        {
            // Arrange

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.GetEvents() as OkObjectResult;

            // Assert an "OK" result with the correct events is returned
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result);

            // Get the events from the db
            var dbEvents = this.dbContext.Events.ToList();

            var resultValues = result.Value as List<EventListingModel>;
            Assert.AreEqual(dbEvents.Count(), resultValues.Count());

            var firstResult = resultValues[0];
            var firstEvent = dbEvents[0];
            Assert.AreEqual(firstEvent.Name, firstResult.Name);

            var secondResult = resultValues[1];
            var secondEvent = dbEvents[1];
            Assert.AreEqual(secondEvent.Name, secondResult.Name);
        }

        [Test]
        public void Test_Events_GetEventById()
        {
            // Arrange: get the "Dev Conference" event from the db
            var devConfEvent = this.testDb.EventDevConf;

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.GetEventById(devConfEvent.Id) 
                as OkObjectResult;
            Assert.IsNotNull(result);

            // Assert an "OK" result with the correct event is returned
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            var resultValue = result.Value as EventListingModel;
            Assert.AreEqual(devConfEvent.Id, resultValue.Id);
            Assert.AreEqual(devConfEvent.Name, resultValue.Name);
        }

        [Test]
        public void Test_Events_Create()
        {
            // Arrange: get the events count before the creation
            int eventsCountBefore = this.dbContext.Events.Count();

            // Set UserMaria as a currently logged in user
            TestingUtils.AssignCurrentUserForController(
                this.eventsController, this.testDb.UserMaria);

            // Create a new event binding model
            var newEventData = new EventBindingModel()
            {
                Name = "New Event " + DateTime.Now.Ticks,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3).AddDays(1),
                TotalTickets = 500,
                PricePerTicket = 20
            };

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.CreateEvent(newEventData) 
                as CreatedAtActionResult;
            Assert.IsNotNull(result);

            // Assert a "Created" result with the correct event data is returned
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);

            var resultValue = result.Value as EventListingModel;
            Assert.IsTrue(resultValue.Id > 0);
            Assert.AreEqual(newEventData.Name, resultValue.Name);
            Assert.AreEqual(newEventData.Place, resultValue.Place);
            Assert.AreEqual(newEventData.Start.ToString("dd/MM/yyyy HH:mm"), resultValue.Start);
            Assert.AreEqual(newEventData.End.ToString("dd/MM/yyyy HH:mm"), resultValue.End);
            Assert.AreEqual(newEventData.PricePerTicket, resultValue.PricePerTicket);
            Assert.AreEqual(newEventData.TotalTickets, resultValue.TotalTickets);

            // Assert the new event is added to the db correctly
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.That(eventsCountAfter == eventsCountBefore + 1);

            var newEventFromDb =
                this.dbContext.Events.FirstOrDefault(e => e.Name == newEventData.Name);
            Assert.IsTrue(newEventFromDb.Id > 0);
            Assert.AreEqual(newEventData.Place, newEventFromDb.Place);
            Assert.AreEqual(newEventData.Start, newEventFromDb.Start);
            Assert.AreEqual(newEventData.End, newEventFromDb.End);
            Assert.AreEqual(newEventData.PricePerTicket, newEventFromDb.PricePerTicket);
            Assert.AreEqual(newEventData.TotalTickets, newEventFromDb.TotalTickets);
        }

        [Test]
        public void Test_Put_ValidId()
        {
            // Arrange: set UserMaria as a currently logged in user
            TestingUtils.AssignCurrentUserForController(
                this.eventsController, this.testDb.UserMaria);

            // Create a new event in the database for editing
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

            // Create an event binding model with changed event name
            var changedEvent = new EventBindingModel()
            {
                Name = "House Party" + DateTime.Now.Ticks,
                Place = newEvent.Place,
                Start = newEvent.Start,
                End = newEvent.End,
                TotalTickets = newEvent.TotalTickets,
                PricePerTicket = newEvent.PricePerTicket
            };

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.PutEvent(newEvent.Id, changedEvent) 
                as NoContentResult;
            Assert.IsNotNull(result);

            // Assert a "NoContent" result is returned
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);

            // Assert the event in the database has a changed name
            var newEventFromDb = this.dbContext.Events.Find(newEvent.Id);
            Assert.AreEqual(newEventFromDb.Name, changedEvent.Name);
        }

        [Test]
        public void Test_Put_InvalidId()
        {
            // Arrange: create an event binding model 
            var changedEvent = new EventBindingModel();

            var invalidId = -1;

            // Act: invoke the controller method with invalid id and cast the result
            var result = this.eventsController.PutEvent(invalidId, changedEvent) 
                as NotFoundObjectResult;
            Assert.IsNotNull(result);

            // Assert a "NotFound" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

            var resultValue = result.Value as ResponseMsg;
            Assert.AreEqual($"Event #{invalidId} not found.", resultValue.Message);
        }

        [Test]
        public void Test_Put_UnauthorizedUser()
        {
            // Arrange: set UserMaria as currently logged in user
            TestingUtils.AssignCurrentUserForController(
                this.eventsController, this.testDb.UserMaria);
            
            // Get the "OpenFest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            // Create an event binding model 
            var changedEvent = new EventBindingModel(); ;

            // Act: invoke the controller method with invalid id and cast the result
            var result = this.eventsController
                .PutEvent(openFestEvent.Id, changedEvent) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);

            // Assert an "Unauthorized" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            var resultValue = result.Value as ResponseMsg;
            Assert.AreEqual($"Cannot edit event, when not an owner.", resultValue.Message);
        }

        [Test]
        public void Test_Patch_ValidId()
        {
            // Arrange: set UserMaria as currently logged in user
            TestingUtils.AssignCurrentUserForController(
                this.eventsController, this.testDb.UserMaria);

            // Create a new event in the database for partial editing
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

            // Create an event model with changed event name
            var changedEvent = new PatchEventModel()
            {
                Name = "House Party" + DateTime.Now.Ticks
            };

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.PatchEvent(newEvent.Id, changedEvent) 
                as NoContentResult;
            Assert.IsNotNull(result);

            // Assert a "NoContent" result with is returned
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);

            // Assert the event in the database has a changed name
            var newEventFromDb = this.dbContext.Events.Find(newEvent.Id);
            Assert.AreEqual(newEventFromDb.Name, changedEvent.Name);
        }

        [Test]
        public void Test_Patch_InvalidId()
        {
            // Arrange: create an event model
            var changedEvent = new PatchEventModel();

            var invalidId = -1;

            // Act: invoke the controller method with an invalid id and cast the result
            var result = this.eventsController.PatchEvent(invalidId, changedEvent) as NotFoundObjectResult;
            Assert.IsNotNull(result);

            // Assert a "NotFound" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

            var resultValue = result.Value as ResponseMsg;
            Assert.AreEqual($"Event #{invalidId} not found.", resultValue.Message);
        }

        [Test]
        public void Test_Patch_UnauthorizedUser()
        {
            // Arrange: get the "OpenFest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            // Set UserMaria as currently logged in user
            TestingUtils.AssignCurrentUserForController(this.eventsController, this.testDb.UserMaria);

            // Create an event model
            var changedEvent = new PatchEventModel();

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.PatchEvent(openFestEvent.Id, changedEvent) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);

            // Assert an "Unauthorized" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            var resultValue = result.Value as ResponseMsg;
            Assert.AreEqual($"Cannot edit event, when not an owner.", resultValue.Message);
        }

        [Test]
        public void Test_Delete_ValidId()
        {
            // Arrange: set UserMaria as currently logged in user
            TestingUtils.AssignCurrentUserForController(this.eventsController, this.testDb.UserMaria);

            // Create a new event in the database for deleting
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

            // Get events count before the deletion
            int eventsCountBefore = this.dbContext.Events.Count();

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.DeleteEvent(newEvent.Id) as OkObjectResult;
            Assert.IsNotNull(result);

            // Assert an "OK" result with the deleted event is returned
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            var resultValue = result.Value as EventListingModel;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Id, newEvent.Id);
            Assert.AreEqual(resultValue.Name, newEvent.Name);
            Assert.AreEqual(resultValue.Place, newEvent.Place);

            // Assert the event is deleted from the database
            int eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore - 1, eventsCountAfter);
            Assert.IsNull(this.dbContext.Events.Find(newEvent.Id));
        }

        [Test]
        public void Test_Delete_InvalidId()
        {
            // Set UserMaria as currently logged in user
            TestingUtils.AssignCurrentUserForController(this.eventsController, this.testDb.UserMaria);

            int invalidId = -1;

            // Act: invoke the controller method with an invalid id and cast the result
            var result = this.eventsController.DeleteEvent(invalidId) as NotFoundObjectResult;
            Assert.IsNotNull(result);

            // Assert a "NotFound" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

            var resultValue = result.Value as ResponseMsg;
            Assert.AreEqual($"Event #{invalidId} not found.", resultValue.Message);
        }

        [Test]
        public void Test_Delete_UnauthorizedUser()
        {
            // Arrange: get the "OpenFest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            // Set UserMaria as currently logged in user
            TestingUtils.AssignCurrentUserForController(this.eventsController, this.testDb.UserMaria);

            // Act: invoke the controller method and cast the result
            var result = this.eventsController.DeleteEvent(openFestEvent.Id) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);

            // Assert an "Unauthorized" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            var resultValue = result.Value as ResponseMsg;
            Assert.AreEqual($"Cannot delete event, when not an owner.", resultValue.Message);

            // Assert the event is not deleted from the database
            Assert.IsNotNull(this.dbContext.Events.Find(openFestEvent.Id));
        }
    }
}
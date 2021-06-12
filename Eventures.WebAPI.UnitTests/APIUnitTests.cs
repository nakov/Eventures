using Eventures.App.Data;
using Eventures.App.Models;
using Eventures.UnitTests;
using Eventures.WebAPI.Controllers;
using Eventures.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Eventures.WebAPI.UnitTests
{
    public class APIUnitTests
    {
        TestDb testDb;
        ApplicationDbContext dbContext;
        EventsController controller;

        [OneTimeSetUp]
        public void Setup()
        {
            testDb = new TestDb();
            dbContext = testDb.CreateDbContext();
            controller = new EventsController(dbContext);
        }

        [Test]
        public void Test_Events_GetCount()
        {
            // Arrange

            // Act
            var result = controller.GetEventsCount();

            // Assert
            Assert.AreEqual(this.dbContext.Events.Count(), result);
        }

        [Test]
        public void Test_Events_GetEvents()
        {
            // Arrange

            // Act
            var result = controller.GetEvents() as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            var resultValues = result.Value as IEnumerable<ApiEventViewModel>;
            Assert.AreEqual(this.dbContext.Events.Count(), resultValues.Count());
        }

        [Test]
        public void Test_Events_GetEventById()
        {
            // Arrange
            int eventId = this.testDb.EventSoftuniada.Id;

            // Act
            var result = controller.GetEventById(eventId) as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            var resultValue = result.Value as ApiEventViewModel;
            Assert.AreEqual(this.testDb.EventSoftuniada.Id, resultValue.Id);
        }

        [Test]
        public void Test_Events_Create()
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
            TestDb.AssignCurrentUserForController(controller, testDb.UserMaria);

            // Act
            var result = controller.CreateEvent(newEventData) as CreatedAtActionResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);

            // Check response data
            var resultValue = result.Value as ApiEventViewModel;
            Assert.IsTrue(resultValue.Id > 0);
            Assert.AreEqual(newEventData.Place, resultValue.Place);
            Assert.AreEqual(newEventData.Start, resultValue.Start);
            Assert.AreEqual(newEventData.End, resultValue.End);
            Assert.AreEqual(newEventData.PricePerTicket, resultValue.PricePerTicket);
            Assert.AreEqual(newEventData.TotalTickets, resultValue.TotalTickets);

            // Check the new event in the database
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
        public void Test_Edit_ValidId()
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

            // Create event binding model with different event name
            var changedEvent = new EventCreateBindingModel()
            {
                Name = "House Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m
            };

            TestDb.AssignCurrentUserForController(controller, testDb.UserMaria);

            // Act
            var result = controller.PutEvent(newEvent.Id, changedEvent) as NoContentResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);

            // Assert the event in the database has correct data
            var newEventFromDb =
               dbContext.Events.FirstOrDefault(e => e.Name == changedEvent.Name);
            Assert.AreEqual(newEventFromDb.Place, changedEvent.Place);
            Assert.AreEqual(newEventFromDb.Start, changedEvent.Start);
            Assert.AreEqual(newEventFromDb.End, changedEvent.End);
            Assert.AreEqual(newEventFromDb.PricePerTicket, changedEvent.PricePerTicket);
            Assert.AreEqual(newEventFromDb.TotalTickets, changedEvent.TotalTickets);
        }

        [Test]
        public void Test_Edit_InvalidId()
        {
            // Arrange: create a new event in the database for deleting
            TestDb.AssignCurrentUserForController(controller, testDb.UserMaria);

            // Create event binding model with different event name
            var changedEvent = new EventCreateBindingModel()
            {
                Name = "House Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m
            };
            var invalidId = -1;

            // Act: make request with invalid id
            var result = controller.PutEvent(invalidId, changedEvent) as NotFoundResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void Test_Edit_UnauthorizedUser()
        {
            // Arrange: create a new user in the database to use for authentication
            var user = new EventuresUser()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.bg",
                UserName = "test"
            };
            dbContext.Add(user);

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

            // Assign the newly-created user to the controller
            TestDb.AssignCurrentUserForController(controller, user);

            // Create event binding model with different event name
            var changedEvent = new EventCreateBindingModel()
            {
                Name = "House Party" + DateTime.Now.Ticks,
                Place = "Ibiza",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 20,
                PricePerTicket = 120.00m
            };

            // Act
            var result = controller.PutEvent(newEvent.Id, changedEvent) as UnauthorizedResult;

            // Assert user is unauthorized
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            // Assert event is not edited in the database
            var newEventFromDb =
               dbContext.Events.FirstOrDefault(e => e.Name == newEvent.Name);
            Assert.AreEqual(newEvent.Place, newEventFromDb.Place);
        }

        [Test]
        public void Test_Delete_ValidId()
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
                OwnerId = testDb.UserMaria.Id
            };
            dbContext.Add(newEvent);
            dbContext.SaveChanges();

            TestDb.AssignCurrentUserForController(controller, testDb.UserMaria);
            int eventsCountBefore = dbContext.Events.Count();
            
            // Act
            var result = controller.DeleteEvent(newEvent.Id) as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            // Assert the event is deleted from the database
            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountBefore == eventsCountAfter + 1);
            Assert.That(this.dbContext.Events.Find(newEvent.Id) == null);

            // Assert the response holds the event data
            var resultValue = result.Value as ApiEventViewModel;
            Assert.IsNotNull(resultValue);
            Assert.That(resultValue.Id == newEvent.Id);
            Assert.That(resultValue.Name == newEvent.Name);
            Assert.That(resultValue.Place == newEvent.Place);
        }

        [Test]
        public void Test_Delete_InvalidId()
        {
            // Arrange: create a new event in the DB for deleting
            TestDb.AssignCurrentUserForController(controller, testDb.UserMaria);
           
            int eventsCountBefore = dbContext.Events.Count();
            int invalidId = -1;
            // Act: create request with invalid id
            var result = controller.DeleteEvent(invalidId) as NotFoundResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            int eventsCountAfter = dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public void Test_Delete_UnauthorizedUser()
        {
            // Arrange: create a new user in the database to use for authentication
            var user = new EventuresUser()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.bg",
                UserName = "test"
            };
            dbContext.Add(user);
            
            // Arrange: create a new event in the database for deleting
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

            int eventsCountBefore = dbContext.Events.Count();

            // Assign the newly-created user to the controller
            TestDb.AssignCurrentUserForController(controller, user);
            
            // Act
            var result = controller.DeleteEvent(newEvent.Id) as UnauthorizedResult;

            // Assert user is unauthorized
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            // Assert the event is not deleted from the database
            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountBefore == eventsCountAfter);
        }
    }
}
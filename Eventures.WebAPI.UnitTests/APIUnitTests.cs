using Eventures.App.Data;
using Eventures.App.Models;
using Eventures.UnitTests;
using Eventures.WebAPI.Controllers;
using Eventures.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Models;

namespace Eventures.WebAPI.UnitTests
{
    public class APIUnitTests
    {
        TestDb testDb;
        ApplicationDbContext dbContext;
        EventsController controller;
        UsersController usersController;

        [OneTimeSetUp]
        public void Setup()
        {
            testDb = new TestDb();
            dbContext = testDb.CreateDbContext();
            controller = new EventsController(dbContext);

            // Get configuration from appsettings.json file in the Web API project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            usersController = new UsersController(dbContext, configuration);
        }

        [Test]
        public async Task Test_User_Register()
        {
            // Arrange: create a new register model
            var newUser = new ApiRegisterModel()
            {
                Username = "newUser",
                FirstName = "Peter",
                LastName = "Petrov",
                Email = "pesho.petrov@abv.bg",
                Password = "password1",
                ConfirmPassword = "password1"
            };
            var usersBefore = this.dbContext.Users.Count();

            // Act
            var result = await usersController.Register(newUser) as OkObjectResult;

            // Assert the user is registered and logged-in successfully
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            var resultValues = result.Value as Response;
            Assert.AreEqual("Success", resultValues.Status);
            Assert.AreEqual("User created successfully!", resultValues.Message);

            var usersAfter = this.dbContext.Users.Count();
            Assert.That(usersAfter == usersBefore + 1);
        }

        [Test]
        public async Task Test_User_Login()
        {
            // Arrange: create a login model
            var user = new ApiLoginModel()
            {
                Username = this.testDb.UserMaria.UserName,
                Password = this.testDb.UserMaria.UserName
            };

            // Act
            var result = await usersController.Login(user) as OkObjectResult;

            // Assert the user is logged in and has a valid token
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            var resultValue = result.Value as ResponseWithToken;
            Assert.IsNotNull(resultValue.Token);
            Assert.AreNotEqual("1/1/0001 12:00:00 AM", resultValue.Expiration);
        }

        [Test]
        public void Test_User_GetAllUsers()
        {
            //Arrange

            //Act
            var result = usersController.GetAll() as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            var resultValue = result.Value as IEnumerable<ApiUserViewModel>;
            Assert.AreEqual(this.dbContext.Users.Count(), resultValue.Count());
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
            // Arrange: get the id of the "Softuniada" event
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
            // Arrange: create a new event binding model
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
            // Arrange: create event binding model with different event name
            var changedName = "Softuniada 2021 (New Edition)";
            var changedEvent = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 200,
                PricePerTicket = 12.00m
            };
            var invalidId = -1;

            // Act: make request with invalid id
            var result = controller.PutEvent(invalidId, changedEvent) as NotFoundObjectResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void Test_Edit_UnauthorizedUser()
        {
            // Arrange: get the "Open Fest" event with owner UserPeter
            var openFestEvent = this.testDb.EventOpenFest;

            // Assign UserMaria to the controller
            TestDb.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            // Create event binding model with different event name
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
            var result = controller.PutEvent(openFestEvent.Id, changedEvent) as UnauthorizedObjectResult;

            // Assert user is unauthorized
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            // Assert event is not edited in the database
            var newEventFromDb =
               dbContext.Events.FirstOrDefault(e => e.Name == openFestEvent.Name);
            Assert.AreEqual(openFestEvent.Place, newEventFromDb.Place);
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
            // Arrange: get the "Open Fest" event with owner UserPeter
            var openFestEvent = this.testDb.EventOpenFest;

            // Assign UserMaria to the controller
            TestDb.AssignCurrentUserForController(controller, this.testDb.UserMaria);

            int eventsCountBefore = dbContext.Events.Count();

            // Act
            var result = controller.DeleteEvent(openFestEvent.Id) as UnauthorizedResult;

            // Assert user is unauthorized
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            // Assert the event is not deleted from the database
            int eventsCountAfter = dbContext.Events.Count();
            Assert.That(eventsCountBefore == eventsCountAfter);
        }
    }
}
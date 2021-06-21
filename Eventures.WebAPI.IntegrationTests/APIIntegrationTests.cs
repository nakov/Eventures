using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Eventures.App.Data;
using Eventures.App.Models;
using Eventures.UnitTests;
using Eventures.WebAPI.IntegraionTests;
using Eventures.WebAPI.Models;
using WebApi.Models;

namespace Eventures.WebAPI.IntegrationTests
{
    public class APIIntegrationTests
    {
        TestDb testDb;
        ApplicationDbContext dbContext;
        TestingWebApiFactory testFactory;
        HttpClient httpClient;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
            this.testFactory = new TestingWebApiFactory(testDb);
            this.httpClient = testFactory.Client;
            await this.testFactory.AuthenticateAsync();
        }

        [Test]
        public async Task Test_Users_GetAll()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("api/users");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var resposeContent = response.Content.ReadAsAsync<List<ApiUserViewModel>>();
            var responseResult = resposeContent.Result;
            Assert.AreEqual(this.dbContext.Users.Count(), responseResult.Count());
            Assert.AreEqual(this.dbContext.Users.FirstOrDefault().UserName, responseResult.FirstOrDefault().Username);
        }

        [Test]
        public async Task Test_Events_GetAll()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("api/events");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Get response content with events
            var responseContent = response.Content.ReadAsAsync<List<ApiEventViewModel>>().Result;
            Assert.AreEqual(this.dbContext.Events.Count(), responseContent.Count());
            Assert.AreEqual(this.dbContext.Events.FirstOrDefault().Name, responseContent.FirstOrDefault().Name);
            Assert.AreEqual(this.dbContext.Events.LastOrDefault().Name, responseContent.LastOrDefault().Name);
        }

        [Test]
        public async Task Test_Events_GetEventById_ValidId()
        {
            // Arrange: get the "Softuniada" event id
            int eventId = this.testDb.EventSoftuniada.Id;

            // Act
            var response = await this.httpClient.GetAsync($"api/events/{eventId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = response.Content.ReadAsAsync<ApiEventViewModel>().Result;
            Assert.AreEqual(this.dbContext.Events.FirstOrDefault().Name, responseContent.Name);
        }

        [Test]
        public async Task Test_Events_GetEventById_InvalidId()
        {
            // Arrange
            int invalidEventId = -1;

            // Act
            var response = await this.httpClient.GetAsync($"api/events/{invalidEventId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_Events_CreateEvent_ValidData()
        {
            // Arrange: create a new binding model
            var eventName = "Party";
            var eventPlace = "Beach";
            var newEvent = new EventCreateBindingModel()
            {
                Name = eventName,
                Place = eventPlace,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };
            var eventsCountBefore = this.dbContext.Events.Count();

            // Act: send a POST request with the new event binding model
            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/events/create", newEvent);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore + 1, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_CreateEvent_InvalidData()
        {
            // Arrange: create event binding model with invalid name
            var invalidEventName = string.Empty;
            var eventPlace = "Beach";
            var newEvent = new EventCreateBindingModel()
            {
                Name = invalidEventName,
                Place = eventPlace,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };
            var eventsCountBefore = this.dbContext.Events.Count();

            // Act
            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/events/create", newEvent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, postResponse.StatusCode);

            var postResponseContent = await postResponse.Content.ReadAsStringAsync();
            Assert.That(postResponseContent.Contains("The Name field is required."));

            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_EditEvent_ValidId()
        {
            // Arrange: get the "Softuniada 2021" event 
            var softuniadaEvent = this.testDb.EventSoftuniada;
            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == softuniadaEvent.Id);

            // Create new event binding model, where only the event name is changed
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

            var eventsCountBefore = this.dbContext.Events.Count();

            // Act: send PUT request with the changed event
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{eventInDb.Id}", changedEvent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, putResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);

            eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == softuniadaEvent.Id);
            Assert.AreEqual(changedName, eventInDb.Name);
        }

        [Test]
        public async Task Test_Events_EditEvent_InvalidId()
        {
            // Arrange
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

            // Act
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{invalidId}", changedEvent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, putResponse.StatusCode);
        }

        [Test]
        public async Task Test_Events_EditEvent_EditEventOfAnotherUser()
        {
            // Note that current user is UserMaria
            // Arrange: get the "Open Fest" event with owner UserPeter
            var openFestEvent = this.testDb.EventOpenFest;

            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == openFestEvent.Id);

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
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{eventInDb.Id}", changedEvent);

            // Assert user is unauthroized because UserMaria is not the owner of the "Open Fest" event
            Assert.AreEqual(HttpStatusCode.Unauthorized, putResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == openFestEvent.Id);
            Assert.AreEqual(openFestEvent.Name, eventInDb.Name);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_ValidId()
        {
            // Arrange: create a new event in the database for deleting
            var newEvent = new Event()
            {
                Name = "Party",
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20,
                OwnerId = this.testDb.UserMaria.Id
            };
            this.dbContext.Add(newEvent);
            this.dbContext.SaveChanges();
            
            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == newEvent.Id);

            var eventsCountBefore = this.dbContext.Events.Count();

            // Act: send DELETE request
            var deleteResponse = await this.httpClient.DeleteAsync(
                $"/api/events/{eventInDb.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deleteResponseContent = deleteResponse.Content.ReadAsAsync<ApiEventViewModel>().Result;
            Assert.AreEqual(eventInDb.Name, deleteResponseContent.Name);

            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore - 1, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_InvalidId()
        {
            // Arrange
            var invalidEventId = -1;

            // Act
            var deleteResponse = await this.httpClient.DeleteAsync(
                $"/api/events/{invalidEventId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_DeleteEventOfAnotherUser()
        {
            // Note that current user is UserMaria
            // Arrange: get the "Open Fest" event with owner UserPeter
            var openFestEvent = this.testDb.EventOpenFest;

            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == openFestEvent.Id);

            // Act
            var deleteResponse = await httpClient.DeleteAsync(
                $"/api/events/{eventInDb.Id}");

            // Assert user is unauthroized because UserMaria is not the owner of the "Open Fest" event
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);
            Assert.That(this.dbContext.Events.Contains(eventInDb));
        }
    }
}
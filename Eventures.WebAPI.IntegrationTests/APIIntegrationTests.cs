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
            var response = await this.httpClient.GetAsync("api/users");
            var resposeContent = response.Content.ReadAsAsync<List<ApiUserViewModel>>();
            var responseResult = resposeContent.Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(this.dbContext.Users.Count(), responseResult.Count());
            Assert.AreEqual(this.dbContext.Users.FirstOrDefault().UserName, responseResult.FirstOrDefault().Username);
        }

        [Test]
        public async Task Test_Events_GetAll()
        {
            await RefreshDb();
            var response = await this.httpClient.GetAsync("api/events");
            var responseContent = response.Content.ReadAsAsync<List<ApiEventViewModel>>();
            var responseResult = responseContent.Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(this.dbContext.Events.Count(), responseResult.Count());
            Assert.AreEqual(this.dbContext.Events.FirstOrDefault().Name, responseResult.FirstOrDefault().Name);
            Assert.AreEqual(this.dbContext.Events.LastOrDefault().Name, responseResult.LastOrDefault().Name);
        }

        [Test]
        public async Task Test_Events_GetEventById_ValidId()
        {
            await RefreshDb();
            int eventId = 1;
            var response = await this.httpClient.GetAsync($"api/events/{eventId}");
            var responseContent = response.Content.ReadAsAsync<ApiEventViewModel>();
            var responseResult = responseContent.Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(this.dbContext.Events.FirstOrDefault().Name, responseResult.Name);
        }

        [Test]
        public async Task Test_Events_GetEventById_InvalidId()
        {
            int invalidEventId = -1;
            var response = await this.httpClient.GetAsync($"api/events/{invalidEventId}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_Events_CreateEvent_ValidData() //Owner is null
        {
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
           
            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/events/create", newEvent);
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore + 1, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_CreateEvent_InvalidData()
        {
            var eventName = string.Empty;
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

            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/events/create", newEvent);
            Assert.AreEqual(HttpStatusCode.BadRequest, postResponse.StatusCode);

            var postResponseContent = await postResponse.Content.ReadAsStringAsync();
            Assert.That(postResponseContent.Contains("The Name field is required."));
            
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_EditEvent_ValidId()
        {
            var newEvent = new EventCreateBindingModel()
            {
                Name = "Party",
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };

            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/events/create", newEvent);
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);
            var postResponseContent = postResponse.Content.ReadAsAsync<ApiEventViewModel>();
            var postResponseResult = postResponseContent.Result;

            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == postResponseResult.Id);

            var changedName = "Best Party";
            var changedEvent = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };

            var eventsCountBefore = this.dbContext.Events.Count();

            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{eventInDb.Id}", changedEvent);
            Assert.AreEqual(HttpStatusCode.NoContent, putResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);

            eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == postResponseResult.Id);
            Assert.AreEqual(changedName, eventInDb.Name);
        }

        [Test]
        public async Task Test_Events_EditEvent_InvalidId()
        {
            var changedName = "Best Party";
            var changedEvent = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };

            var invalidId = -1;
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{invalidId}", changedEvent);
            Assert.AreEqual(HttpStatusCode.NotFound, putResponse.StatusCode);
        }

        [Test]
        public async Task Test_Events_EditEvent_EditEventOfAnotherUser()
        {
            var newEvent = new Event()
            {
                Name = "Party",
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };
            this.dbContext.Add(newEvent);
            this.dbContext.SaveChanges();

            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == newEvent.Id);

            var changedName = "Best Party";
            var changedEvent = new EventCreateBindingModel()
            {
                Name = changedName,
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };

            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{eventInDb.Id}", changedEvent);
            Assert.AreEqual(HttpStatusCode.Unauthorized, putResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == newEvent.Id);
            Assert.AreEqual(newEvent.Name, eventInDb.Name);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_ValidId()
        {
            var newEvent = new EventCreateBindingModel()
            {
                Name = "Party",
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };

            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/events/create", newEvent);
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);
            var postResponseContent = postResponse.Content.ReadAsAsync<ApiEventViewModel>();
            var postResponseResult = postResponseContent.Result;

            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == postResponseResult.Id);

            var eventsCountBefore = this.dbContext.Events.Count();

            var deleteResponse = await this.httpClient.DeleteAsync(
                $"/api/events/{eventInDb.Id}");
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deleteResponseContent = deleteResponse.Content.ReadAsAsync<ApiEventViewModel>();
            var deleteResponseResult = deleteResponseContent.Result;
            Assert.AreEqual(eventInDb.Name, deleteResponseResult.Name);

            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore - 1, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_InvalidId()
        {
            var invalidEventId = -1;
            var deleteResponse = await this.httpClient.DeleteAsync(
                $"/api/events/{invalidEventId}");
            Assert.AreEqual(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_DeleteEventOfAnotherUser()
        {
            var newEvent = new Event()
            {
                Name = "Party",
                Place = "Beach",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddHours(3),
                TotalTickets = 120,
                PricePerTicket = 20
            };
            this.dbContext.Add(newEvent);
            this.dbContext.SaveChanges();

            var eventInDb = this.dbContext.Events.FirstOrDefault(x => x.Id == newEvent.Id);

            // Put event data
            var deleteResponse = await httpClient.DeleteAsync(
                $"/api/events/{eventInDb.Id}");
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);

            Assert.That(this.dbContext.Events.Contains(eventInDb));
        }

        private async Task RefreshDb()
        {
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
            this.testFactory = new TestingWebApiFactory(testDb);
            this.httpClient = testFactory.Client;
            await this.testFactory.AuthenticateAsync();
        }
    }
}
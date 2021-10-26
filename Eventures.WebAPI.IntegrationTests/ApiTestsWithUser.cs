using System;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Eventures.Data;
using Eventures.WebAPI.Models;
using Eventures.WebAPI.Models.User;
using Eventures.WebAPI.Models.Event;

using NUnit.Framework;
using Newtonsoft.Json;

namespace Eventures.WebAPI.IntegrationTests
{
    public class ApiTestsWithUser : ApiTestsBase
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await base.AuthenticateAsync();
        }

        [Test]
        public async Task Test_Users_GetAll()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("/api/users");

            // Assert users are returned successfully
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var resposeContent = response.Content.ReadAsAsync<List<UserListingModel>>();
            var responseResult = resposeContent.Result;
            Assert.AreEqual(this.dbContext.Users.Count(), 
                responseResult.Count());
        }

        [Test]
        public async Task Test_Events_GetAll()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("/api/events");

            // Assert the events are returned successfully
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = response.Content.ReadAsAsync<List<EventListingModel>>().Result;
            Assert.AreEqual(this.dbContext.Events.Count(), responseContent.Count());
        }

        [Test]
        public async Task Test_Events_GetEventById_ValidId()
        {
            // Arrange: get the "Dev Conference" event
            var devConfEventId = this.testDb.EventDevConf.Id;

            // Act
            var response = await this.httpClient.GetAsync($"/api/events/{devConfEventId}");

            // Assert the returned event is correct
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = response.Content
                .ReadAsAsync<EventListingModel>().Result;
            Assert.AreEqual(dbContext.Events.Find(devConfEventId).Name, 
                responseContent.Name);
        }

        [Test]
        public async Task Test_Events_GetEventById_InvalidId()
        {
            // Arrange
            int invalidEventId = -1;

            // Act: send request wuth invalid id
            var response = await this.httpClient.GetAsync($"/api/events/{invalidEventId}");

            // Assert "NotFound" is returned
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual($"Event #{invalidEventId} not found.", responseContent.Message);
        }

        [Test]
        public async Task Test_Events_CreateEvent_ValidData()
        {
            // Arrange: create a new binding model
            var eventName = "Party";
            var eventPlace = "Beach";
            var newEvent = new EventBindingModel()
            {
                Name = eventName,
                Place = eventPlace,
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now.AddDays(2),
                TotalTickets = 120,
                PricePerTicket = 20.00M
            };

            var eventsCountBefore = this.dbContext.Events.Count();

            // Act: send a POST request with the new event binding model
            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/events/create", newEvent);

            // Assert the event is created successfully
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore + 1, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_CreateEvent_InvalidData()
        {
            // Arrange: create event binding model
            // with invalid name: name == empty string
            var invalidEventName = string.Empty;
            var newEvent = new EventBindingModel()
            {
                Name = invalidEventName,
                Place = "Beach",
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now.AddDays(2),
                TotalTickets = 120,
                PricePerTicket = 20
            };
            var eventsCountBefore = this.dbContext.Events.Count();

            // Act
            var postResponse = await this.httpClient.PostAsJsonAsync("/api/events/create", newEvent);

            // Assert a "BadRequest" is returned
            Assert.AreEqual(HttpStatusCode.BadRequest, postResponse.StatusCode);

            var postResponseContent = await postResponse.Content.ReadAsStringAsync();
            Assert.That(postResponseContent.Contains("The Name field is required."));

            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_EditEvent_ValidId()
        {
            // Arrange: get the "Dev Conference" event 
            var devConfEvent = this.testDb.EventDevConf;

            // Create new event binding model, where only the event name is changed
            var changedName = "Dev Conference (New Edition)";
            var changedEvent = new EventBindingModel()
            {
                Name = changedName,
                Place = devConfEvent.Place,
                Start = devConfEvent.Start,
                End = devConfEvent.End,
                TotalTickets = devConfEvent.TotalTickets,
                PricePerTicket = devConfEvent.PricePerTicket
            };

            // Act: send PUT request with the changed event
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{devConfEvent.Id}", changedEvent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, putResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            var eventInDbAfter = this.dbContext.Events.Find(devConfEvent.Id);
            Assert.AreEqual(changedName, eventInDbAfter.Name);
        }

        [Test]
        public async Task Test_Events_EditEvent_InvalidId()
        {
            // Arrange: create an event binding model with changed name
            var changedName = "Dev Conference (New Edition)";
            var changedEvent = new EventBindingModel()
            {
                Name = changedName,
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 200,
                PricePerTicket = 12.00m
            };

            var invalidId = -1;

            // Act: send a PUT request with invalid id
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{invalidId}", changedEvent);

            // Assert "NotFound" is returned
            Assert.AreEqual(HttpStatusCode.NotFound, putResponse.StatusCode);

            var putResponseContent = await putResponse.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual($"Event #{invalidId} not found.", putResponseContent.Message);
        }

        [Test]
        public async Task Test_Events_EditEvent_InvalidData()
        {
            // Arrange: get the "Dev Conference" event 
            var devConfEvent = this.testDb.EventDevConf;

            // Create new event binding model, where only the event name is changed
            // The name is invalid
            var changedName = string.Empty;
            var changedEvent = new EventBindingModel()
            {
                Name = changedName,
                Place = devConfEvent.Place,
                Start = devConfEvent.Start,
                End = devConfEvent.End,
                TotalTickets = devConfEvent.TotalTickets,
                PricePerTicket = devConfEvent.PricePerTicket
            };

            // Act: send PUT request with the changed event
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{devConfEvent.Id}", changedEvent);

            // Assert "BadRequest" is returned
            Assert.AreEqual(HttpStatusCode.BadRequest, putResponse.StatusCode);

            // Assert the event in not edited in the database
            this.dbContext = this.testDb.CreateDbContext();
            var eventInDbAfter = this.dbContext.Events.Find(devConfEvent.Id);
            Assert.AreEqual(devConfEvent.Name, eventInDbAfter.Name);
        }

        [Test]
        public async Task Test_Events_EditEvent_EditEventOfAnotherUser()
        {
            // Note that current user is UserMaria
            // Arrange: get the "OpenFest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            // Create an event binding model with changed name
            var changedName = "OpenFest (New Edition)";
            var changedEvent = new EventBindingModel()
            {
                Name = changedName,
                Place = openFestEvent.Place,
                Start = openFestEvent.Start,
                End = openFestEvent.End,
                TotalTickets = openFestEvent.TotalTickets,
                PricePerTicket = openFestEvent.PricePerTicket
            };

            // Act
            var putResponse = await this.httpClient.PutAsJsonAsync(
                $"/api/events/{openFestEvent.Id}", changedEvent);

            // Assert user is unauthorized because
            // UserMaria is not the owner of the "Open Fest" event
            Assert.AreEqual(HttpStatusCode.Unauthorized, putResponse.StatusCode);

            var putResponseContent = await putResponse.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual($"Cannot edit event, when not an owner.", 
                putResponseContent.Message);

            // Assert the event in not edited in the database
            this.dbContext = this.testDb.CreateDbContext();
            var eventInDbAfter = this.dbContext.Events.Find(openFestEvent.Id);
            Assert.AreEqual(openFestEvent.Name, eventInDbAfter.Name);
        }

        [Test]
        public async Task Test_Events_PartialEditEvent_ValidId()
        {
            // Arrange: get the "Dev Conference" event 
            var devConfEvent = this.testDb.EventDevConf;

            // Create new event model, where only the event name is changed
            var changedName = "Dev Conference (New Edition)";
            var changedEvent = new PatchEventModel()
            {
                Name = changedName
            };

            // Act: send PATCH request with the changed event as StringContent
            var requestContent = new StringContent
                (JsonConvert.SerializeObject(changedEvent), 
                Encoding.UTF8, 
                "application/json");
            var patchResponse = await this.httpClient.PatchAsync(
                $"/api/events/{devConfEvent.Id}", requestContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, patchResponse.StatusCode);

            this.dbContext = this.testDb.CreateDbContext();
            var eventInDbAfter = this.dbContext.Events.Find(devConfEvent.Id);
            Assert.AreEqual(changedName, eventInDbAfter.Name);
            Assert.AreEqual(devConfEvent.Place, eventInDbAfter.Place);
        }

        [Test]
        public async Task Test_Events_PartialEditEvent_InvalidId()
        {
            // Arrange: create an event model with changed name
            var changedName = "Dev Conference (New Edition)";
            var changedEvent = new PatchEventModel()
            {
                Name = changedName
            };

            var invalidId = -1;
            var requestContent = new StringContent(JsonConvert.SerializeObject(changedEvent), Encoding.UTF8, "application/json");

            // Act: send a PATCH request with invalid id
            var patchResponse = await this.httpClient.PatchAsync(
                $"/api/events/{invalidId}", requestContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, patchResponse.StatusCode);

            var putResponseContent = await patchResponse.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual($"Event #{invalidId} not found.", putResponseContent.Message);
        }

        [Test]
        public async Task Test_Events_PartialEditEvent_InvalidData()
        {
            // Arrange: get the "Dev Conference" event 
            var devConferenceEvent = this.testDb.EventDevConf;

            var eventInDbBefore = this.dbContext.Events.Find(devConferenceEvent.Id);

            // Create an event model with changed name
            // The name is invalid
            var changedName = string.Empty;
            var changedEvent = new PatchEventModel()
            {
                Name = changedName
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(changedEvent), Encoding.UTF8, "application/json");

            // Act: send PATCH request with the changed event
            var putResponse = await this.httpClient.PatchAsync(
                $"/api/events/{eventInDbBefore.Id}", requestContent);

            // Assert a "Bad Request" is returned
            Assert.AreEqual(HttpStatusCode.BadRequest, putResponse.StatusCode);

            // Assert the event in not edited in the database
            this.dbContext = this.testDb.CreateDbContext();
            var eventInDbAfter = this.dbContext.Events.Find(devConferenceEvent.Id);
            Assert.AreEqual(eventInDbBefore.Name, eventInDbAfter.Name);
        }

        [Test]
        public async Task Test_Events_PartialEditEvent_EditEventOfAnotherUser()
        {
            // Note that current user is UserMaria
            // Arrange: get the "Open Fest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            var eventInDbBefore = this.dbContext.Events.FirstOrDefault(x => x.Id == openFestEvent.Id);

            var changedName = "OpenFest (New Edition)";
            var changedEvent = new PatchEventModel()
            {
                Name = changedName
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(changedEvent), Encoding.UTF8, "application/json");
            
            // Act
            var patchResponse = await this.httpClient.PatchAsync(
                $"/api/events/{eventInDbBefore.Id}", requestContent);

            // Assert user is unauthroized because UserMaria is not the owner of the "Open Fest" event
            Assert.AreEqual(HttpStatusCode.Unauthorized, patchResponse.StatusCode);

            var patchResponseContent = await patchResponse.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual($"Cannot edit event, when not an owner.", patchResponseContent.Message);

            this.dbContext = this.testDb.CreateDbContext();
            var eventInDbAfter = this.dbContext.Events.Find(openFestEvent.Id);
            Assert.AreEqual(eventInDbBefore.Name, eventInDbAfter.Name);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_ValidId()
        {
            // Arrange: create a new event in the database for deleting
            var newEvent = new Event()
            {
                Name = "Party",
                Place = "Beach",
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now.AddDays(2),
                TotalTickets = 120,
                PricePerTicket = 20,
                OwnerId = this.testDb.UserMaria.Id
            };
            this.dbContext.Add(newEvent);
            this.dbContext.SaveChanges();

            var eventsCountBefore = this.dbContext.Events.Count();

            // Act: send a DELETE request
            var deleteResponse = await this.httpClient.DeleteAsync(
                $"/api/events/{newEvent.Id}");

            // Assert the deletion is successfull
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deleteResponseContent = deleteResponse.Content
                .ReadAsAsync<EventListingModel>().Result;
            Assert.AreEqual(newEvent.Name, deleteResponseContent.Name);

            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore - 1, eventsCountAfter);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_InvalidId()
        {
            // Arrange
            var invalidId = -1;

            // Act: send a DELETE request with invalid id
            var deleteResponse = await this.httpClient.DeleteAsync(
                $"/api/events/{invalidId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            var deleteResponseContent = await deleteResponse.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual($"Event #{invalidId} not found.", deleteResponseContent.Message);
        }

        [Test]
        public async Task Test_Events_DeleteEvent_DeleteEventOfAnotherUser()
        {
            // Note that current user is UserMaria
            // Arrange: get the "OpenFest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            var eventInDb = this.dbContext.Events.Find(openFestEvent.Id);

            // Act
            var deleteResponse = await httpClient.DeleteAsync(
                $"/api/events/{eventInDb.Id}");

            // Assert user is unauthroized because UserMaria is not the owner of the "OpenFest" event
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);

            var deleteResponseContent = await deleteResponse.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual($"Cannot delete event, when not an owner.", deleteResponseContent.Message);

            Assert.IsNotNull(this.dbContext.Events.Find(eventInDb.Id));
        }
    }
}
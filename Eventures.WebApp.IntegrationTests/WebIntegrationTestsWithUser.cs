using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

using Eventures.Data;

using NUnit.Framework;

namespace Eventures.WebApp.IntegrationTests
{
    public class WebIntegrationTestsWithUser : WebIntegrationTestsBase
    {
        [OneTimeSetUp]
        public async Task SetUpUser()
        {
            // Login user "UserMaria"
            await LoginUser(this.testDb.UserMaria.UserName, this.testDb.UserMaria.UserName);
        }

        [Test]
        public async Task Test_UserLogout()
        {
            // Go to the "Logout" page
            var response = await this.httpClient.GetAsync(
                 "/Identity/Account/Logout");
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("Log out"));
            
            var antiForgeryToken = ExtractAntiForgeryToken(responseBody);
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "__RequestVerificationToken", antiForgeryToken }
                });

            // Send a POST request to log out the user
            var postResponse = await this.httpClient.PostAsync(
                 "/Identity/Account/Logout?returnUrl=%2F", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            // Assert that the user was redirected to the "Home" page
            Assert.AreEqual("/", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(postResponseBody.Contains("<h1>Eventures: Events and Tickets</h1>"));
        }

        [Test]
        public async Task Test_HomePage_LoggedIn()
        {
            // Arrange
            
            // Act
            var response = await this.httpClient.GetAsync("/");

            // Assert the user is logged-in
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Assert a "Welcome" message appears
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains($"Welcome, {this.testDb.UserMaria.UserName}"));

            // Assert all events count is correct
            var allEventsCount = this.dbContext.Events.Count();
            Assert.That(responseBody.Contains($"We already have <b>{allEventsCount}</b> events on our Eventures App!"));

            // Assert user's events count is correct
            var currentUser = this.testDb.UserMaria;
            var userEventsCount = this.dbContext.Events.Where(e => e.OwnerId == currentUser.Id).Count();
            Assert.That(responseBody.Contains($"You have <b>{userEventsCount}</b> event(s)!"));
        }

        [Test]
        public async Task Test_EventsPage_ViewAllEvents_Authenticated()
        {
            // Arrange

            // Act: go to the "All Events" page
            var response = await this.httpClient.GetAsync("/Events/All");

            // Assert the user is on the page
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("<h1>All Events</h1>"));

            // Assert that events are all listed correctly on the page
            foreach (var e in this.testDb.CreateDbContext().Events)
            {
                Assert.That(responseBody.Contains(e.Name));
            }
        }

        [Test]
        public async Task Test_EventsPage_CreateEvent_ValidData()
        {
            // Arrange: go to the "Create Event" page
            var response = await this.httpClient.GetAsync("/Events/Create");
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("<h1>Create New Event</h1>"));

            // Prepare content for creating a new event
            var eventName = "Party" + DateTime.Now.Ticks;
            var eventPlace = "Beach";
            var startDate = DateTime.Now.AddDays(1).ToString();
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", eventName },
                    { "Place", eventPlace },
                    { "Start", DateTime.Now.AddDays(1).ToString()},
                    { "End", DateTime.Now.AddDays(1).AddHours(3).ToString()},
                    { "TotalTickets", "120"},
                    { "PricePerTicket", "20"}
                });

            // Act: send a POST request with event data
            var postResponse = await this.httpClient.PostAsync(
                "/Events/Create", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            // Assert user is redirected to "All Events" page
            Assert.AreEqual("/Events/All", postResponse.RequestMessage.RequestUri.LocalPath);

            // Assert the new event appears on the page
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();
            Assert.That(postResponseBody, Does.Contain(eventName));
            Assert.That(postResponseBody, Does.Contain(eventPlace));

            // Assert the event is also created in the database
            Assert.IsTrue(this.testDb.CreateDbContext().Events.Any(e => e.Name == eventName));
        }

        [Test]
        public async Task Test_EventsPage_CreateEvent_InvalidData()
        {
            // Arrange: go to the "Create Event" page
            var response = await this.httpClient.GetAsync("/Events/Create");
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("<h1>Create New Event</h1>"));
            var eventsCountBefore = this.testDb.CreateDbContext().Events.Count();

            // Prepare content for creating a new event with invalid name: name == empty string
            var invalidName = string.Empty;
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", invalidName },
                    { "Place", "Beach" },
                    { "Start", DateTime.Now.ToString()},
                    { "End", DateTime.Now.AddHours(3).ToString()},
                    { "TotalTickets", "120"},
                    { "PricePerTicket", "20"}
                });

            // Act: send a POST request with event data
            var postResponse = await this.httpClient.PostAsync(
                "/Events/Create", postContent);

            // Assert the user is not redirected and an error message appears
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();
            Assert.AreEqual("/Events/Create", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(postResponseBody, Does.Contain($"The Name field is required."));

            // Assert events count in the database is not changed
            var eventsCountAfter = this.testDb.CreateDbContext().Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }

        [Test]
        public async Task Test_DeletePage_DeleteEventOfAnotherUser_ShouldBeUnsuccessful()
        {
            // Arrange: get the "EventOpenFest" event with owner GuestUser
            var openFestEvent = this.testDb.EventOpenFest;

            // Go to "All Events" page and assert the event exists
            var allresponse = await this.httpClient.GetAsync("/Events/All");
            var allresponseBody = await allresponse.Content.ReadAsStringAsync();
            Assert.That(allresponseBody.Contains(openFestEvent.Name));

            // Act: load the "Delete Event" page for the event
            var deleteResponse = await this.httpClient.GetAsync(
               $"/Events/Delete/{openFestEvent.Id}");
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deleteResponseBody = await deleteResponse.Content.ReadAsStringAsync();

            // Assert that the an "Oops, you don't have access here." message appears
            // because our current logged-in user is UserMaria, not the owner of the event
            Assert.That(deleteResponseBody.Contains("Oops, you don't have access here."));
        }

        [Test]
        public async Task Test_EventsPage_DeleteEvent()
        {
            // Arrange: create a new event in the database for deleting
            var dbContext = this.testDb.CreateDbContext();
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
            dbContext.Add(newEvent);
            dbContext.SaveChanges();

            // Go to "All Events" page and assert the event exists
            var allResponse = await this.httpClient.GetAsync("/Events/All");
            var allResponseBody = await allResponse.Content.ReadAsStringAsync();
            Assert.That(allResponseBody.Contains(newEvent.Name));

            // Load the "Delete Event" page with the new event id
            var deleteResponse = await this.httpClient.GetAsync(
               $"/Events/Delete/{newEvent.Id}");
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);
            var deleteResponseBody = await deleteResponse.Content.ReadAsStringAsync();

            // Prepare request data with the anti-forgery token
            var antiForgeryToken = ExtractAntiForgeryToken(deleteResponseBody);
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "__RequestVerificationToken", antiForgeryToken }
                });

            // Act: send a POST request to confirm deletion
            var postResponse = await this.httpClient.PostAsync(
                $"/Events/Delete/{newEvent.Id}", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            // Assert the user is redirected to "All Events" page and the deleted event doesn't appear
            Assert.AreEqual("/Events/All", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(!postResponseBody.Contains(newEvent.Name));
            
            // Assert that the event was deleted from the database
            dbContext = this.testDb.CreateDbContext();
            var deletedEvent = dbContext.Events.Find(newEvent.Id);
            Assert.That(deletedEvent == null);
        }

        [Test]
        public async Task Test_EditPage_EditEventOfAnotherUser_ShouldBeUnsuccessful()
        {
            // Arrange: get the "EventOpenFest" event with owner UserPeter
            var eventOpenFest = this.testDb.EventOpenFest;

            // Go to "All Events" page and assert the event exists
            var allResponse = await this.httpClient.GetAsync("/Events/All");
            Assert.AreEqual(HttpStatusCode.OK, allResponse.StatusCode);
            var allResponseBody = await allResponse.Content.ReadAsStringAsync();
            Assert.That(allResponseBody.Contains(eventOpenFest.Name));

            // Act: go to the "Edit Event" page with the new event id
            var editResponse = await this.httpClient.GetAsync(
               $"/Events/Edit/{eventOpenFest.Id}");
            Assert.AreEqual(HttpStatusCode.OK, editResponse.StatusCode);
            var editResponseBody = await editResponse.Content.ReadAsStringAsync();

            // Assert that an "Oops, you don't have access here." message appears on the page
            // because our current logged-in user is UserMaria, not the owner of the event
            Assert.That(editResponseBody.Contains("Oops, you don't have access here."));
        }

        [Test]
        public async Task Test_EventsPage_EditEvent_ValidData()
        {
            // Arrange: get the "Dev Conference" event
            var devConfEvent = this.testDb.EventDevConf;

            // Go to "All Events" page and assert the event exists
            var allResponse = await this.httpClient.GetAsync("/Events/All");
            var allResponseBody = await allResponse.Content.ReadAsStringAsync();
            Assert.That(allResponseBody.Contains(devConfEvent.Name));

            // Go to the "Edit Event" page with the new event id
            var editResponse = await this.httpClient.GetAsync(
               $"/Events/Edit/{devConfEvent.Id}");
            Assert.AreEqual(HttpStatusCode.OK, editResponse.StatusCode);

            // Prepare request content with changed event name
            var editedEventName = "Dev Conference (New Edition)";
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", editedEventName },
                    { "Place", devConfEvent.Place },
                    { "Start", devConfEvent.Start.ToString()},
                    { "End", devConfEvent.End.ToString()},
                    { "TotalTickets", devConfEvent.TotalTickets.ToString()},
                    { "PricePerTicket", devConfEvent.PricePerTicket
                        .ToString(CultureInfo.GetCultureInfo("en-US"))}
                });

            // Act: send a POST request with the new event data
            var postResponse = await this.httpClient.PostAsync(
                $"/Events/Edit/{devConfEvent.Id}", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            // Assert the user is redirected to the "All Events" page
            Assert.AreEqual("/Events/All", postResponse.RequestMessage.RequestUri.LocalPath);

            // Assert the event has a changed name
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();
            Assert.That(postResponseBody, Does.Contain(editedEventName));

            // Assert the event is also edited in the database
            var eventInDb = this.testDb.CreateDbContext().Events.FirstOrDefault(x => x.Id == devConfEvent.Id);
            Assert.AreEqual(eventInDb.Name, editedEventName);
        }

        [Test]
        public async Task Test_EventsPage_EditEvent_InvalidData()
        {
            // Arrange: get the "Dev Conference" event
            var devConfEvent = this.testDb.EventDevConf;

            // Go to the "All Events" page and assert the event exists
            var allResponse = await this.httpClient.GetAsync("/Events/All");
            var allResponseBody = await allResponse.Content.ReadAsStringAsync();
            Assert.That(allResponseBody.Contains(devConfEvent.Name));

            // Go to the "Edit Event" page with the new event id
            var editResponse = await this.httpClient.GetAsync(
               $"/Events/Edit/{devConfEvent.Id}");
            Assert.AreEqual(HttpStatusCode.OK, editResponse.StatusCode);

            // Prepare request content with invalid event name: name == empty string
            var invalidEventName = string.Empty;
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", invalidEventName },
                    { "Place", devConfEvent.Place },
                    { "Start", devConfEvent.Start.ToString()},
                    { "End", devConfEvent.End.ToString()},
                    { "TotalTickets", devConfEvent.TotalTickets.ToString()},
                    { "PricePerTicket", devConfEvent.PricePerTicket.ToString()}
                });

            // Act: send a POST request with the new event data
            var postResponse = await this.httpClient.PostAsync(
                $"/Events/Edit/{devConfEvent.Id}", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            // Assert the user is on the same page
            Assert.AreEqual($"/Events/Edit/{devConfEvent.Id}", postResponse.RequestMessage.RequestUri.LocalPath);

            // Assert the event is not edited in the database
            var eventInDb = this.testDb.CreateDbContext().Events.FirstOrDefault(x => x.Id == devConfEvent.Id);
            Assert.AreEqual(eventInDb.Name, devConfEvent.Name);
        }

        public async Task LoginUser(string username, string password)
        {
            // Go to the "Login" page
            var response =
                await this.httpClient.GetAsync("/Identity/Account/Login");
            var responseBody =
                await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(responseBody.Contains("<h1>Log in</h1>"));

            // Prepare request content for login
            var antiForgeryToken = ExtractAntiForgeryToken(responseBody);
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Input.Username", username },
                    { "Input.Password", password },
                    { "Input.RememberMe", "false" },
                    { "__RequestVerificationToken", antiForgeryToken }
                });

            // Act: send a POST request with the login data
            var postResponse = await this.httpClient.PostAsync(
                "/Identity/Account/Login", postContent);

            // Assert login is successful
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            // Assert the user is redirected to the "Home" page
            Assert.AreEqual("/", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(postResponseBody.Contains($"Welcome, {username}"));
        }
    }
}

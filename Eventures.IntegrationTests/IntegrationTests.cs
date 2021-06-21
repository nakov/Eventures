using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Eventures.UnitTests;
using Eventures.App.Data;

namespace Eventures.IntegrationTests
{
    public class IntegrationTests
    {
        HttpClient httpClient;
        TestingWebAppFactory testingWebAppFactory;
        TestDb testDb;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            this.testDb = new TestDb();
            testingWebAppFactory = new TestingWebAppFactory(testDb);
            this.httpClient = testingWebAppFactory.CreateClient();
            // Login UserMaria
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

            // Send a post request to log out client
            var postResponse = await this.httpClient.PostAsync(
                 "/Identity/Account/Logout?returnUrl=%2F", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            // Assert that the client was redirected to the Home page
            Assert.AreEqual("/", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(postResponseBody.Contains("<h1>Eventures: Events and Tickets</h1>"));
        }

        [Test]
        public async Task Test_HomePage_LoggedIn()
        {
            // Arrange
            
            // Act
            var response = await this.httpClient.GetAsync("/");

            // Assert the user is logged-in and there is a "Welcome" message 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains($"Welcome, {this.testDb.UserMaria.UserName}"));
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
            var eventName = "Party";
            var eventPlace = "Beach";
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", eventName },
                    { "Place", eventPlace },
                    { "Start", $"{DateTime.UtcNow}"},
                    { "End", $"{DateTime.UtcNow.AddHours(3)}"},
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
            var lastEvent = this.testDb.CreateDbContext().Events.Last();
            Assert.AreEqual(lastEvent.Name, eventName);
            Assert.AreEqual(lastEvent.Place, eventPlace);
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
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", string.Empty },
                    { "Place", "Beach" },
                    { "Start", $"{DateTime.UtcNow}"},
                    { "End", $"{DateTime.UtcNow.AddHours(3)}"},
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
            // Arrange: get the "EventOpenFest" event with owner UserPeter
            var eventOpenFest = this.testDb.EventOpenFest;

            // Go to "All Events" page and assert the event exists
            var allresponse = await this.httpClient.GetAsync("/Events/All");
            var allresponseBody = await allresponse.Content.ReadAsStringAsync();
            Assert.That(allresponseBody.Contains(eventOpenFest.Name));

            // Act: load the "Delete Event" page for the event
            var deleteResponse = await this.httpClient.GetAsync(
               $"/Events/Delete/{eventOpenFest.Id}");
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deleteResponseBody = await deleteResponse.Content.ReadAsStringAsync();

            // Assert that the an "Event not found." message appears
            // because our current logged-in user is UserMaria, not the owner of the event
            Assert.That(deleteResponseBody.Contains("Event not found."));
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
            var allresponse = await this.httpClient.GetAsync("/Events/All");
            var allresponseBody = await allresponse.Content.ReadAsStringAsync();
            Assert.That(allresponseBody.Contains(newEvent.Name));

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

            // Assert that an "Event not found." message appears on the page
            // because our current logged-in user is UserMaria, not the owner of the event
            Assert.That(editResponseBody.Contains("Event not found."));
        }

        [Test]
        public async Task Test_EventsPage_EditEvent_ValidData()
        {
            // Arrange: get the "Softuniada 2021" event
            var softuniadaEvent = this.testDb.EventSoftuniada;

            // Go to "All Events" page and assert the event exists
            var allresponse = await this.httpClient.GetAsync("/Events/All");
            var allresponseBody = await allresponse.Content.ReadAsStringAsync();
            Assert.That(allresponseBody.Contains(softuniadaEvent.Name));

            // Go to the "Edit Event" page with the new event id
            var editResponse = await this.httpClient.GetAsync(
               $"/Events/Edit/{softuniadaEvent.Id}");
            Assert.AreEqual(HttpStatusCode.OK, editResponse.StatusCode);

            // Prepare request content with changed event name
            var editedEventName = "Softuniada 2021 (New Edition)";
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", editedEventName },
                    { "Place", softuniadaEvent.Place },
                    { "Start", softuniadaEvent.Start.ToString()},
                    { "End", softuniadaEvent.End.ToString()},
                    { "TotalTickets", softuniadaEvent.TotalTickets.ToString()},
                    { "PricePerTicket", softuniadaEvent.PricePerTicket.ToString()}
                });

            // Act: send a POST request with the new event data
            var postResponse = await this.httpClient.PostAsync(
                $"/Events/Edit/{softuniadaEvent.Id}", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            // Assert the user is redirected to the "All Events" page
            Assert.AreEqual("/Events/All", postResponse.RequestMessage.RequestUri.LocalPath);

            // Assert the event has a new name
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();
            Assert.That(postResponseBody, Does.Contain(editedEventName));

            // Assert the event is also edited in the database
            var eventInDb = this.testDb.CreateDbContext().Events.FirstOrDefault(x => x.Id == softuniadaEvent.Id);
            Assert.AreEqual(eventInDb.Name, editedEventName);
        }

        [Test]
        public async Task Test_EventsPage_EditEvent_InvalidData()
        {
            // Arrange: get the "Softuniada 2021" event
            var softuniadaEvent = this.testDb.EventSoftuniada;

            // Go to the "All Events" page and assert the event exists
            var allresponse = await this.httpClient.GetAsync("/Events/All");
            var allresponseBody = await allresponse.Content.ReadAsStringAsync();
            Assert.That(allresponseBody.Contains(softuniadaEvent.Name));

            // Go to the "Edit Event" page with the new event id
            var editResponse = await this.httpClient.GetAsync(
               $"/Events/Edit/{softuniadaEvent.Id}");
            Assert.AreEqual(HttpStatusCode.OK, editResponse.StatusCode);

            // Prepare request content with invalid event name: name == empty string
            var invalidEventName = string.Empty;
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Name", invalidEventName },
                    { "Place", softuniadaEvent.Place },
                    { "Start", softuniadaEvent.Start.ToString()},
                    { "End", softuniadaEvent.End.ToString()},
                    { "TotalTickets", softuniadaEvent.TotalTickets.ToString()},
                    { "PricePerTicket", softuniadaEvent.PricePerTicket.ToString()}
                });

            // Act: send a POST request with the new event data
            var postResponse = await this.httpClient.PostAsync(
                $"/Events/Edit/{softuniadaEvent.Id}", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            // Assert the user is on the same page
            Assert.AreEqual($"/Events/Edit/{softuniadaEvent.Id}", postResponse.RequestMessage.RequestUri.LocalPath);

            // Assert the event is not edited in the database
            var eventInDb = this.testDb.CreateDbContext().Events.FirstOrDefault(x => x.Id == softuniadaEvent.Id);
            Assert.AreEqual(eventInDb.Name, softuniadaEvent.Name);
        }

        public async Task LoginUser(string username, string password)
        {
            // Go to the "Login" page
            var loginFormResponse =
                await this.httpClient.GetAsync("/Identity/Account/Login");
            var loginFormResponseBody =
                await loginFormResponse.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, loginFormResponse.StatusCode);
            Assert.That(loginFormResponseBody.Contains("<h1>Log in</h1>"));

            // Prepare request content for login
            var antiForgeryToken = ExtractAntiForgeryToken(loginFormResponseBody);
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

        private static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            Match match = Regex.Match(htmlResponseText, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }
    }
}

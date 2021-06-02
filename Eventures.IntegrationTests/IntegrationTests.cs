using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
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
            await LoginUser(this.testDb.UserMaria.UserName, this.testDb.UserMaria.UserName);
        }

        [Test]
        public async Task Test_UserLogout()
        {







            // TODO: use logout with CSRF token
            // POST /Identity/Account/Logout?returnUrl=%2F 








            // Always redirected to Log Out page when tested (logged and not-logged in users)
            var response = await this.httpClient.GetAsync(
                "/Identity/Account/Logout");
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("Log out"));
        }

        [Test]
        public async Task Test_HomePage_LoggedIn()
        {
            // Arrange
            
            // Act
            var response = await this.httpClient.GetAsync("/");

            // Assert that login is required
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains($"Welcome, {this.testDb.UserMaria.UserName}"));
        }

        [Test]
        public async Task Test_EventsPage_ViewAllEvents_Authenticated()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("/Events/All");

            // Assert that events are listed correctly
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("<h1>All Events</h1>"));
            foreach (var e in this.testDb.CreateDbContext().Events)
            {
                Assert.That(responseBody.Contains(e.Name));
            }
        }

        [Test]
        public async Task Test_EventsPage_CreateEvent_ValidData()
        {
            // Go to the "Event Create" page
            var response = await this.httpClient.GetAsync("/Events/Create");
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("<h1>Create New Event</h1>"));

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

            // Post event data
            var postResponse = await this.httpClient.PostAsync(
                "/Events/Create", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            // You should be redirected to "All Events" page
            Assert.AreEqual("/Events/All", postResponse.RequestMessage.RequestUri.LocalPath);

            // The event should be created
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();
            Assert.That(postResponseBody, Does.Contain(eventName));
            Assert.That(postResponseBody, Does.Contain(eventPlace));
        }

        [Test]
        public async Task Test_EventsPage_CreateEvent_InvalidData()
        {
            var response = await this.httpClient.GetAsync("/Events/Create");
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody.Contains("<h1>Create New Event</h1>"));

            // Send invalid event data: name == empty string
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

            // Page should not be redirected and an error message should appear
            var postResponse = await this.httpClient.PostAsync(
                "/Events/Create", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();
            Assert.AreEqual("/Events/Create", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(postResponseBody, Does.Contain($"The Name field is required."));
        }

        [Test]
        public async Task Test_EventsPage_DeleteEvent()
        {
            // Arrange: create a new event in the DB for deleting
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

            // Load the "Delete Event" page for the new event id
            var deleteResponse = await this.httpClient.GetAsync(
               $"/Events/Delete/{newEvent.Id}");
            deleteResponse.EnsureSuccessStatusCode();
            var deleteResponseBody = await deleteResponse.Content.ReadAsStringAsync();

            // Send POST request to confirm deletion
            var antiForgeryToken = ExtractAntiForgeryToken(deleteResponseBody);
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "__RequestVerificationToken", antiForgeryToken }
                });

            var postResponse = await this.httpClient.PostAsync(
                $"/Events/Delete/{newEvent.Id}", postContent);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            // You should be redirected to "All Events" page and event should not exist
            Assert.AreEqual("/Events/All", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(!postResponseBody.Contains(newEvent.Name));
            dbContext = this.testDb.CreateDbContext();
            var deletedEvent = dbContext.Events.Find(newEvent.Id);
            Assert.That(deletedEvent == null);
        }

        public async Task LoginUser(string username, string password)
        {
            // Load the login form
            var loginFormResponse =
                await this.httpClient.GetAsync("/Identity/Account/Login");
            var loginFormResponseBody =
                await loginFormResponse.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, loginFormResponse.StatusCode);
            Assert.That(loginFormResponseBody.Contains("<h1>Log in</h1>"));

            // Fill the login form and send a post request
            var antiForgeryToken = ExtractAntiForgeryToken(loginFormResponseBody);
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Input.Username", username },
                    { "Input.Password", password },
                    { "Input.RememberMe", "false" },
                    { "__RequestVerificationToken", antiForgeryToken }
                });

            var postResponse = await this.httpClient.PostAsync(
                "/Identity/Account/Login", postContent);

            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var responseBody = await postResponse.Content.ReadAsStringAsync();
            // Assert that the client was redirected to the Home page
            Assert.AreEqual("/", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(responseBody.Contains($"Welcome, {username}"));
        }

        private static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            Match match = Regex.Match(htmlResponseText, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }
    }
}

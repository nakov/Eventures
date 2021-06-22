using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using NUnit.Framework;

using Eventures.Tests.Common;

namespace Eventures.IntegrationTests
{
    public class IntegrationTests_Anonymous
    {
        TestDb testDb;
        TestEventuresApp testEventuresApp;
        HttpClient httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.testDb = new TestDb();
            this.testEventuresApp = new TestEventuresApp(testDb);
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(this.testEventuresApp.ServerUri);
        }

        [Test]
        public async Task Test_UserRegistration()
        {
            // Load the registration form
            var response = await this.httpClient.GetAsync("/Identity/Account/Register");
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(responseBody, Does.Contain("<h1>Register</h1>"));

            // Fill the registration form and submit it
            string username = "user" + DateTime.Now.Ticks;
            string password = "pass" + DateTime.Now.Ticks;
            var antiForgeryToken = ExtractAntiForgeryToken(responseBody);
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Input.Username", username },
                    { "Input.Email", username + "@testmail.com" },
                    { "Input.Password", password },
                    { "Input.ConfirmPassword", password },
                    { "Input.FirstName", "Test" },
                    { "Input.LastName", "User" },
                    { "__RequestVerificationToken", antiForgeryToken }
                });
            var postResponse = await this.httpClient.PostAsync(
                "/Identity/Account/Register", postContent);
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();
            
            // Assert that the client was redirected to the Home page
            Assert.AreEqual("/", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(postResponseBody, Does.Contain($"Welcome, {username}"));

            //// Assert that the ASP.NET Identity cookie was set correctly by the server
            //postResponse.Headers.TryGetValues("Set-Cookie", out var cookiesSetByRequest);
            //var aspNetIdentityCookie = cookiesSetByRequest.FirstOrDefault(
            //    c => c.Contains(".AspNetCore.Identity.Application"));
            //Assert.NotNull(aspNetIdentityCookie);
        }

        [Test]
        public async Task Test_UserLogin()
        {
            // Load the login form
            var loginFormResponse =
                await this.httpClient.GetAsync("/Identity/Account/Login");
            var loginFormResponseBody =
                await loginFormResponse.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, loginFormResponse.StatusCode);
            Assert.That(loginFormResponseBody.Contains("<h1>Log in</h1>"));

            // Fill the login form and send a POST request
            string username = this.testDb.UserMaria.UserName;
            string password = this.testDb.UserMaria.UserName;
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

            // Assert the login operation succeeded
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
            var responseBody = await postResponse.Content.ReadAsStringAsync();

            // Assert that the client was redirected to the Home page
            Assert.AreEqual("/", postResponse.RequestMessage.RequestUri.LocalPath);
            Assert.That(responseBody.Contains($"Welcome, {username}"));

            // Assert that the ASP.NET Identity cookie was set correctly by the server
            //var requestHeaders = this.httpClient.DefaultRequestHeaders;
            //var cookies = postResponse.Headers.GetValues(HeaderNames.Cookie);
            //var aspNetIdentityCookie = cookies.FirstOrDefault(
            //    c => c.Contains(".AspNetCore.Identity.Application"));
            //Assert.NotNull(aspNetIdentityCookie);
        }

        [Test]
        public async Task Test_HomePage_AnonymousUser()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            string anonymousWelcomeMessage = "<h1>Eventures: Events and Tickets</h1>";
            Assert.That(responseBody, Does.Contain(anonymousWelcomeMessage));
        }

        [Test]
        public async Task Test_EventsPage_ViewAllEvents_AnonymousUser()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("/Events/All");

            // Assert that login is required
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody, Does.Contain("<h1>Log in</h1>"));
        }

        private static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            Match match = Regex.Match(htmlResponseText, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.testEventuresApp.Dispose();
        }
    }
}

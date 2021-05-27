using System.Net.Http;
using NUnit.Framework;

using Eventures.UnitTests;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace Eventures.IntegrationTests
{
    public class IntegrationTests
    {
        HttpClient httpClient;
        TestDb testDb;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.testDb = new TestDb();
            var testingWebAppFactory = new TestingWebAppFactory(testDb);
            this.httpClient = testingWebAppFactory.CreateClient();
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
            var username = "user" + DateTime.Now.Ticks;
            var password = "pass" + DateTime.Now.Ticks;
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
            Assert.AreEqual(HttpStatusCode.Redirect, postResponse.StatusCode);
            Assert.AreEqual(postResponse.Headers.Location, "/");

            // Open the redirected page and check for logged-in user
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
            Assert.That(loginFormResponseBody, Does.Contain("<h1>Log in</h1>"));

            // Fill the login form and submit it
            var antiForgeryToken = ExtractAntiForgeryToken(loginFormResponseBody);
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "Input.Username", this.testDb.UserMaria.UserName },
                    { "Input.Password", this.testDb.UserMaria.UserName },
                    { "__RequestVerificationToken", antiForgeryToken }
                });
            var postRequest = new HttpRequestMessage(
                HttpMethod.Post, "/Identity/Account/Login");
            postRequest.Content = postContent;
            var postResponse = await httpClient.SendAsync(postRequest);
            var responseBody = await postResponse.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.Redirect, postResponse.StatusCode);
            Assert.AreEqual(postResponse.Headers.Location, "/");

            // Open the redirected page and check for logged-in user
        }

        [Test]
        public void Test_UserLogout()
        {
            Assert.Pass();
        }

        [Test]
        public async Task Test_HomePage_AnnonymousUser()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody, Does.Contain(
                "<title>Home Page - Eventures.App</title>"));
        }

        [Test]
        public void Test_HomePage_LoggedIn()
        {
            Assert.Pass();
        }

        [Test]
        public async Task Test_EventsPage_ViewAllEvents_Annonymous()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("/Events/All");

            // Assert that login is required
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody, Does.Contain("<h1>Log in</h1>"));
        }

        [Test]
        public async Task Test_EventsPage_ViewAllEvents_Authenticated()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/Events/All");
            request.Headers.Add("Cookie", ".AspNetCore.Identity.Application=CfDJ8NKP8FVNgLdNr5vijRhs5aVMcx_mQrymT8QgcOZe5yNPh8gsrxsgVUpU9O4-jFMg_9omspy8nMV7zLEgEV8k_XApgpDlErl0Uy7YMYX76U55L-ZePWfWmdtxQ17l2Z4eOdUFIeGcKc_n-FBYCL5qoboz50vL9jg4rY31LRLTd4Spc5GbI9bkP51Jf1QBuTV23TzMYZpJpBi1M3ys-VE3ERDJLsHSYiGNYbgraPkJmbNAWGNhZIvPnopTiQiR9rxeYC7AlNSbiDdSMEN2F_Q6VZLTxnyOWvGtSPEBaIBmICgean5QuV3Al1RVWvIc-btqN7iuv4y3_Bs-I1_SWSBz6l2MkSUx8NDfTld9UYsjCrG1tZ6r0UvlWA0hjRo_d7grSPszQC_zMbIHIfSviDbdWrLPIfqIIiEA5QGkvqbP6AjmAq84jp18AeBvrX20MmPFRJGZ-TBKCGgV38Hq3QBD2MQEu8NrLRnlsy5ffm6vPY-635xqfV_Dt18_HP0KTy_dlblXZNT1kuUZfcxP-jGNCQrDe9JSjQ3S01kzGA5It00WOzY2jKX12q1VyHN0perALhuC9AptDswkm4bqVxmFIQBkmhDwNke7Oi8f4vufWUBDDKIT5aLBy-yAsW0V96i14KlD4VY3E8mhtgpSJtA2Du7hA21u_F6z7WHExhHIj_GEvcjK0Q0Z1m0AoN1JZQJTC-z1TP7z6gRxCY5LOOuaUrs");

            // Act
            var response = await this.httpClient.SendAsync(request);

            // Assert that login is required
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody, Does.Contain("<h1>All Events</h1>"));
        }

        [Test]
        public void Test_EventsPage_CreateEvent_ValidData()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_EventsPage_CreateEvent_InvalidData()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_EventsPage_DeleteEvent()
        {
            Assert.Pass();
        }

        private static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            Match match = Regex.Match(htmlResponseText, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }
    }
}

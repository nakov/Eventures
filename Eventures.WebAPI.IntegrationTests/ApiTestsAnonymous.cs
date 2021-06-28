using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using NUnit.Framework;

using Eventures.WebAPI.Models;

namespace Eventures.WebAPI.IntegrationTests
{
    public class ApiTestsAnonymous : ApiTestsBase
    {
        [Test]
        public async Task Test_Events_GetAll_Unauthorized()
        {
            // Arrange

            // Act: send a GET request
            var response = await httpClient.GetAsync("api/events");

            // Assert the user is unauthorized
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public async Task Test_Events_GetCount()
        {
            // Arrange

            // Act
            var response = await this.httpClient.GetAsync("api/events/count");
            var responseContent = response.Content.ReadAsStringAsync();
            var responseResult = int.Parse(responseContent.Result);

            // Assert the returned events count is correct
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(this.dbContext.Events.Count(), responseResult);
        }

        [Test]
        public async Task Test_Users_Register_ValidData()
        {
            // Arrange: create a new register model
            string username = "user" + DateTime.Now.Ticks;
            string password = "pass" + DateTime.Now.Ticks;
            var newUser = new ApiRegisterModel()
            {
                FirstName = "Test",
                LastName = "User",
                Username = username,
                Password = password,
                ConfirmPassword = password,
                Email = username + "@testmail.com"
            };
            var usersCountBefore = this.dbContext.Users.Count();

            // Act: send a POST request with registration data
            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/users/register", newUser);

            // Assert the user is registered and logged-in successfully
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            var postResponseContent = postResponse.Content.ReadAsAsync<ResponseMsg>();
            var postResponseResult = postResponseContent.Result;
            Assert.AreEqual("User created successfully!", postResponseResult.Message);

            var usersCountAfter = this.dbContext.Users.Count();
            Assert.AreEqual(usersCountBefore + 1, usersCountAfter);
        }

        [Test]
        public async Task Test_Users_Register_InvalidData()
        {
            // Arrange: create a register model with invalid username: username == empty string
            string username = string.Empty;
            string password = "pass" + DateTime.Now.Ticks;
            var newUser = new ApiRegisterModel()
            {
                FirstName = "Test",
                LastName = "User",
                Username = username,
                Password = password,
                ConfirmPassword = password,
                Email = username + "@testmail.com"
            };
            var usersCountBefore = this.dbContext.Users.Count();

            // Act
            var postResponse = await this.httpClient.PostAsJsonAsync(
                "/api/users/register", newUser);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, postResponse.StatusCode);

            var postResponseContent = postResponse.Content.ReadAsStringAsync();
            var postResponseResult = postResponseContent.Result;
            Assert.That(postResponseResult.Contains("Username is required!"));

            var usersCountAfter = this.dbContext.Users.Count();
            Assert.AreEqual(usersCountBefore, usersCountAfter);
        }

        [Test]
        public async Task Test_Users_Login_ValidData()
        {
            // Arrange
            var userMaria = this.testDb.UserMaria;

            // Act
            var postResponse = await httpClient.PostAsJsonAsync("api/users/login", new ApiLoginModel
            {
                Username = userMaria.UserName,
                Password = userMaria.UserName
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);

            var postResponseContent = await postResponse.Content.ReadAsAsync<ResponseWithToken>();
            Assert.IsNotNull(postResponseContent.Token);
            Assert.IsNotNull(postResponseContent.Expiration);
        }

        [Test]
        public async Task Test_Users_Login_InvalidData()
        {
            // Arrange
            var userMaria = this.testDb.UserMaria;
            var wrongPassword = "wrongPass";

            // Act: send a POST request with invalid password
            var postResponse = await httpClient.PostAsJsonAsync("api/users/login", new ApiLoginModel
            {
                Username = userMaria.UserName,
                Password = wrongPassword
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, postResponse.StatusCode);

            var postResponseContent = await postResponse.Content.ReadAsAsync<ResponseMsg>();
            Assert.AreEqual("Invalid username or password!", postResponseContent.Message);
        }
    }
}

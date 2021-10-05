using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using Eventures.WebAPI.Models;
using Eventures.WebAPI.Controllers;
using Eventures.WebAPI.Models.User;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Eventures.WebAPI.UnitTests
{
    public class ApiUserUnitTests : ApiUnitTestsBase
    {
        HomeController homeController;
        UsersController usersController;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this.homeController = new HomeController();

            // Get configuration from appsettings.json file in the Web API project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            this.usersController = new UsersController(this.dbContext, configuration);
        }

        [Test]
        public void Test_Home_Index()
        {
            // Arrange

            // Act
            var result = this.homeController.Index() as LocalRedirectResult;

            // Assert the user is redirected
            Assert.AreEqual(@"/api/docs/index.html", result.Url);
        }

        [Test]
        public async Task Test_User_Register()
        {
            // Arrange: get users count before the registration
            var usersBefore = this.dbContext.Users.Count();
            // Create a new register model
            var newUser = new RegisterModel()
            {
                Username = "user" + DateTime.Now.Ticks.ToString().Substring(10),
                FirstName = "Peter",
                LastName = "Petrov",
                Email = "pesho.petrov@abv.bg",
                Password = "pass",
                ConfirmPassword = "pass"
            };

            // Act: invoke the controller method and cast the result
            var result = await this.usersController.Register(newUser) as OkObjectResult;
            Assert.IsNotNull(result);

            // Assert an "OK" result with a correct message is returned
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            var resultValues = result.Value as ResponseMsg;
            Assert.AreEqual("User created successfully!", resultValues.Message);

            // Assert the user is added to the db
            var usersAfter = this.dbContext.Users.Count();
            Assert.AreEqual(usersBefore + 1, usersAfter);
        }

        [Test]
        public async Task Test_User_Register_ExistingUser()
        {
            // Arrange: get users count before the registration
            var usersBefore = this.dbContext.Users.Count();

            // Get UserMaria from the db
            var userMaria = this.testDb.UserMaria;

            // Create a new register model with UserMaria data
            var newUser = new RegisterModel()
            {
                Username = userMaria.UserName,
                FirstName = userMaria.FirstName,
                LastName = userMaria.LastName,
                Email = userMaria.Email,
                Password = userMaria.PasswordHash,
                ConfirmPassword = userMaria.PasswordHash
            };

            // Act: invoke the controller method and cast the result
            var result = await this.usersController.Register(newUser) as BadRequestObjectResult;
            Assert.IsNotNull(result);

            // Assert a "BadRequest" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            var resultValues = result.Value as ResponseMsg;
            Assert.AreEqual("User already exists!", resultValues.Message);

            // Assert the user is not created in the db
            var usersAfter = this.dbContext.Users.Count();
            Assert.AreEqual(usersBefore, usersAfter);
        }

        [Test]
        public async Task Test_User_Register_UnmatchingPasswords()
        {
            // Arrange: get users count before the registration
            var usersBefore = this.dbContext.Users.Count();

            // Create a new register model with unmatching passwords
            var newUser = new RegisterModel()
            {
                Username = "user" + DateTime.Now.Ticks.ToString().Substring(10),
                FirstName = "Peter",
                LastName = "Petrov",
                Email = "pesho.petrov@abv.bg",
                Password = "pass",
                ConfirmPassword = "anotherPass"
            };

            // Act: invoke the controller method and cast the result
            var result = await this.usersController.Register(newUser) as BadRequestObjectResult;
            Assert.IsNotNull(result);

            // Assert a "BadRequest" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            var resultValues = result.Value as ResponseMsg;
            Assert.AreEqual("Password and Confirm Password don't match!", resultValues.Message);

            // Assert the user is not created in the db
            var usersAfter = this.dbContext.Users.Count();
            Assert.AreEqual(usersBefore, usersAfter);
        }

        [Test]
        public async Task Test_User_Login_ValidCredentials()
        {
            // Arrange: get UserMaria from the db
            var userMariaUsername = this.testDb.UserMaria.UserName;

            // Create a login model
            var user = new LoginModel()
            {
                Username = userMariaUsername,
                Password = userMariaUsername
            };

            // Act: invoke the controller method and cast the result
            var result = await this.usersController.Login(user) as OkObjectResult;
            Assert.IsNotNull(result);

            // Assert an "OK" result with valid data is returned
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            var resultValue = result.Value as ResponseWithToken;
            Assert.IsNotNull(resultValue.Token);
            Assert.AreNotEqual("1/1/0001 12:00:00 AM", resultValue.Expiration);
        }

        [Test]
        public async Task Test_User_Login_InvalidCredentials()
        {
            // Arrange: create a login model with a wrong password
            var user = new LoginModel()
            {
                Username = this.testDb.UserMaria.UserName,
                Password = "invalidPassword"
            };

            // Act: invoke the controller method and cast the result
            var result = await this.usersController.Login(user) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);

            // Assert an "Unauthorized" result with an error message is returned
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);

            var resultValue = result.Value as ResponseMsg;
            Assert.AreEqual("Invalid username or password!", resultValue.Message);
        }

        [Test]
        public void Test_User_GetAllUsers()
        {
            //Arrange

            // Act: invoke the controller method and cast the result
            var result = this.usersController.GetAll() as OkObjectResult;
            Assert.IsNotNull(result);

            // Assert an "OK" result with valid data is returned
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            
            var resultValue = result.Value as IEnumerable<UserListingModel>;
            Assert.AreEqual(this.dbContext.Users.Count(), resultValue.Count());
        }   
    }
}
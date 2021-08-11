using System;
using System.Net.Http;

using System.Threading.Tasks;
using System.Net.Http.Headers;

using NUnit.Framework;

using Eventures.Data;
using Eventures.Tests.Common;
using Eventures.WebAPI.Models;

namespace Eventures.WebAPI.IntegrationTests
{
    public class ApiTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;
        protected TestEventuresApp<Startup> testEventuresApp;
        protected HttpClient httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
            this.testEventuresApp = new TestEventuresApp<Startup>(
                testDb, "../../../../Eventures.WebAPI");
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri(this.testEventuresApp.ServerUri)
            };
        }

        public async Task AuthenticateAsync()
        {
            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetJWTAsync());
        }

        private async Task<string> GetJWTAsync()
        {
            var userMaria = this.testDb.UserMaria;
            var response = await this.httpClient.PostAsJsonAsync("api/users/login",
                new ApiLoginModel
                {
                    Username = userMaria.UserName,
                    Password = userMaria.UserName
                });

            var loginResponse = await response.Content.ReadAsAsync<ResponseWithToken>();

            return loginResponse.Token;
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Stop and dispose the local Web API server
            this.testEventuresApp.Dispose();
        }
    }
}

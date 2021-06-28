using System.Net.Http;

using NUnit.Framework;

using System.Threading.Tasks;
using System.Net.Http.Headers;

using Eventures.Data;
using Eventures.Tests.Common;
using Eventures.WebAPI.Models;
using System;

namespace Eventures.WebAPI.IntegrationTests
{
    public class ApiTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;
        protected TestEventuresApp<Startup> testEventuresApp;
        protected HttpClient httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
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
            var response = await this.httpClient.PostAsJsonAsync("api/users/login", new ApiLoginModel
            {
                Username = userMaria.UserName,
                Password = userMaria.UserName
            });

            var loginResponse = await response.Content.ReadAsAsync<ResponseWithToken>();

            return loginResponse.Token;
        }
    }
}

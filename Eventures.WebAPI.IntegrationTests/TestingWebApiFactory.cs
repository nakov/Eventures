using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Eventures.Data;
using Eventures.WebAPI.Models;
using Eventures.Tests.Common;

namespace Eventures.WebAPI.IntegrationTests
{
    public class TestingWebApiFactory
    {
        private TestDb testDb;
        private readonly HttpClient client;

        public TestingWebApiFactory(TestDb testDb)
        {
            this.testDb = testDb;
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var oldDbContext = services.SingleOrDefault(
                             d => d.ServiceType == typeof(ApplicationDbContext));
                        services.Remove(oldDbContext);
                        services.AddScoped<ApplicationDbContext>(
                            provider => this.testDb.CreateDbContext());
                    });
                });
            client = appFactory.CreateClient();
        }

        public HttpClient Client => this.client;

        public async Task AuthenticateAsync()
        {
            Client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", await GetJWTAsync());
        }

        private async Task<string> GetJWTAsync()
        {
            var userMaria = this.testDb.UserMaria;
            var response = await Client.PostAsJsonAsync("api/users/login", new ApiLoginModel
            {
                Username = userMaria.UserName,
                Password = userMaria.UserName
            });

            var loginResponse = await response.Content.ReadAsAsync<ResponseWithToken>();

            return loginResponse.Token;
        }
    }
}

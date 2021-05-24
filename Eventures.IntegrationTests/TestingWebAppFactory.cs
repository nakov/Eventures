using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Eventures.App;
using Eventures.App.Data;

namespace Eventures.IntegrationTests
{
    public class TestingWebAppFactory : WebApplicationFactory<Startup>
    {
        private ApplicationDbContext dbContext;

        public TestingWebAppFactory(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var oldDbContext = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ApplicationDbContext));
                services.Remove(oldDbContext);
                services.AddScoped<ApplicationDbContext>(provider => this.dbContext);
            });
        }
    }
}

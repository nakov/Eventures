using System;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Eventures.App;
using Eventures.App.Data;
using Eventures.UnitTests;

namespace Eventures.Tests.Common
{
    public class TestEventuresApp : IDisposable
    {
        private TestDb testDb;
        private IHost host;
        public string ServerUri { get; private set; }

        public TestEventuresApp(TestDb testDb)
        {
            this.testDb = testDb;
        
            var hostBuilder = Host.CreateDefaultBuilder();
            hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseContentRoot("../../../../Eventures.App");
                webHostBuilder.UseStartup<Startup>();
                webHostBuilder.ConfigureServices(services =>
                {
                    var oldDbContext = services.SingleOrDefault(
                        descr => descr.ServiceType == typeof(ApplicationDbContext));
                    services.Remove(oldDbContext);
                    services.AddScoped<ApplicationDbContext>(
                        provider => this.testDb.CreateDbContext());
                });
            });
            this.host = hostBuilder.Build();
            this.host.Start();
            var server = this.host.Services.GetRequiredService<IServer>();
            this.ServerUri =
                server.Features.Get<IServerAddressesFeature>()
                .Addresses.FirstOrDefault();
        }

        public void Dispose()
        {
            this.host.Dispose();
        }
    }
}

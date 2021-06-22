using System;
using System.Linq;
using System.Diagnostics;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Eventures.App;
using Eventures.App.Data;

namespace Eventures.Tests.Common
{
    public class TestEventuresApp : IDisposable
    {
        private IHost host;
        public TestDb TestDb { get; set; }
        public string ServerUri { get; private set; }

        public TestEventuresApp(TestDb testDb)
        {
            this.TestDb = testDb;
        
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
                        provider => this.TestDb.CreateDbContext());
                });
                webHostBuilder.UseUrls("http://127.0.0.1:0");
            });
            this.host = hostBuilder.Build();
            this.host.Start();
            var server = this.host.Services.GetRequiredService<IServer>();
            var serverAddresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            this.ServerUri = serverAddresses.Where(a => a.Contains("http://")).FirstOrDefault();
            Debug.WriteLine($"Testing server started: {this.ServerUri}");
        }

        public void Dispose()
        {
            this.host.Dispose();
            Debug.WriteLine($"Testing server stopped: {this.ServerUri}");
        }
    }
}

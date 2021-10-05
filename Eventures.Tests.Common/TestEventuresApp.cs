using System;
using System.Linq;
using System.Diagnostics;

using Eventures.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Eventures.Tests.Common
{
    public class TestEventuresApp<TStartup> : IDisposable where TStartup : class
    {
        private IHost host;

        public TestEventuresApp(TestDb testDb, string appContentRoot)
        {
            this.TestDb = testDb;
        
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder();
            hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseContentRoot(appContentRoot);
                webHostBuilder.UseStartup<TStartup>();
                webHostBuilder.ConfigureServices(services =>
                {
                    var oldDbContext = services.SingleOrDefault(
                        descr => descr.ServiceType == typeof(ApplicationDbContext));
                    services.Remove(oldDbContext);
                    services.AddScoped<ApplicationDbContext>(
                        provider => this.TestDb.CreateDbContext());
                });
                // Use random free TCP port for the Web server
                webHostBuilder.UseUrls("http://127.0.0.1:0");
            });
            this.host = hostBuilder.Build();
            this.host.Start();
            var server = this.host.Services.GetRequiredService<IServer>();
            var serverAddresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            this.ServerUri = serverAddresses.Where(a => a.Contains("http://")).FirstOrDefault();
            Debug.WriteLine($"Testing server started: {this.ServerUri}");
        }

        public TestDb TestDb { get; set; }
        public string ServerUri { get; private set; }

        public void Dispose()
        {
            this.host.Dispose();
            Debug.WriteLine($"Testing server stopped: {this.ServerUri}");
        }
    }
}

using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Eventures.WebApp.Areas.Identity.IdentityHostingStartup))]
namespace Eventures.WebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
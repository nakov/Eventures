using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Eventures.App.Areas.Identity.IdentityHostingStartup))]
namespace Eventures.App.Areas.Identity
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
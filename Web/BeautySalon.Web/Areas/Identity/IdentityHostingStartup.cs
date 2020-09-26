using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BeautySalon.Web.Areas.Identity.IdentityHostingStartup))]

namespace BeautySalon.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}

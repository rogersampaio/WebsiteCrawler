using Microsoft.Win32;
using System.ComponentModel;
using WebsiteCrawler;
using WebsiteCrawler.Controllers;

[assembly: HostingStartup(typeof(AppHost))]
namespace WebsiteCrawler
{
    public class AppHost : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder
        .Configure(app =>
        {
            app.UseSwaggerUI();
        });

        //public override void Configure(Container container)
       // {
            //container.AddScoped<IParsePage, ParsePage>();
       // }
    }
}

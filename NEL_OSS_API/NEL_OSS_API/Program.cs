using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NEL_Common;

namespace NEL_OSS_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, Config.getAppPort());
                })
                .Build();
    }
}

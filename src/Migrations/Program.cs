using Microsoft.Extensions.Hosting;

namespace Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                new Startup(hostContext.Configuration).ConfigureServices(services);
            })
            .Build();
        
        host.Run();
    }      
}
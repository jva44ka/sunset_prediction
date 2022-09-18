using OpenWeatherMap.Worker.HostedServices;
using OpenWeatherMap.Worker.Installers;

namespace OpenWeatherMap.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions(hostContext.Configuration)
                        .AddServices()
                        .AddHostedService<OpenWeatherMapHostedService>();
            })
            .Build();

        host.Run();
    }
}
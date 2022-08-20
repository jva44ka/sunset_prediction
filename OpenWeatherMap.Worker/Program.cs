using OpenWeatherMap.Worker.HostedServices;
using OpenWeatherMap.Worker.Installers;
using TelegramApi.Worker.Installers;

namespace OpenWeatherMap.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    OptionsInstaller.ConfigureServices(services, hostContext.Configuration);
                    ServicesInstaller.ConfigureServices(services);
                    services.AddHostedService<OpenWeatherMapHostedService>();
                })
                .Build();

            host.Run();
        }
    }
}
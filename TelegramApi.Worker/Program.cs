using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegramApi.Worker.HostedServices;
using TelegramApi.Worker.Installers;

namespace TelegramApi.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                OptionsInstaller.ConfigureServices(services, hostContext.Configuration);
                ServicesInstaller.ConfigureServices(services);
                services.AddHostedService<TelegramBackgroundService>();
            })
            .Build();

        host.Run();
    }
}
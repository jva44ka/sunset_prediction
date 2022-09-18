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
                services.AddOptions(hostContext.Configuration)
                        .AddServices()
                        .AddHostedService<TelegramBackgroundService>();
            })
            .Build();

        host.Run();
    }
}
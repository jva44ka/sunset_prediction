using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Installers
{
    public static class ServicesInstaller
    {
        public static IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            return serviceCollection;
        }
    }
}

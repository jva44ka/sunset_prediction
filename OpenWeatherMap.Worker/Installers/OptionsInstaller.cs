using DataAccess.Settings;
using OpenWeatherMap.Client.Settings;

namespace OpenWeatherMap.Worker.Installers;

public static class OptionsInstaller
{
    /// <summary>
    ///     Добавляет в serviceCollection сервисы настроек appSettings.json
    /// </summary>
    public static IServiceCollection ConfigureServices(IServiceCollection serviceCollection, 
        IConfiguration configuration)
    {
        serviceCollection.Configure<DatabaseConnectionSettings>(configuration.GetSection(nameof(DatabaseConnectionSettings)));
        serviceCollection.Configure<OpenWeatherMapApiSettings>(configuration.GetSection(nameof(OpenWeatherMapApiSettings)));
        return serviceCollection;
    }
}
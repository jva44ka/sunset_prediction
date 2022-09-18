using DataAccess.Settings;
using OpenWeatherMap.Client.Settings;
using OpenWeatherMap.Worker.Settings;

namespace OpenWeatherMap.Worker.Installers;

public static class OptionsInstaller
{
    /// <summary>
    ///     Добавляет в serviceCollection сервисы настроек appSettings.json
    /// </summary>
    public static IServiceCollection AddOptions(this IServiceCollection serviceCollection, 
        IConfiguration configuration)
    {
        serviceCollection.Configure<DatabaseConnectionSettings>(configuration.GetSection(nameof(DatabaseConnectionSettings)));
        serviceCollection.Configure<OpenWeatherMapApiSettings>(configuration.GetSection(nameof(OpenWeatherMapApiSettings)));
        serviceCollection.Configure<OpenWeatherMapWorkerSettings>(configuration.GetSection(nameof(OpenWeatherMapWorkerSettings)));
        return serviceCollection;
    }
}
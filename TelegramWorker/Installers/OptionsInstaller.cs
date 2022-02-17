using DataAccess.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramWorker.Settings;

namespace TelegramWorker.Installers
{
    public static class OptionsInstaller
    {
        /// <summary>
        ///     Добавляет в serviceCollection сервисы настроек appSettings.json
        /// </summary>
        public static IServiceCollection ConfigureServices(IServiceCollection serviceCollection, 
                                                           IConfiguration configuration)
        {
            serviceCollection.Configure<DatabaseConnectionSettings>(configuration.GetSection(nameof(DatabaseConnectionSettings)));
            serviceCollection.Configure<TelegramApiSettings>(configuration.GetSection(nameof(TelegramApiSettings)));
            return serviceCollection;
        }
    }
}

using Application.Mappers;
using Application.Mappers.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using DataAccess.ConnectionFactories;
using DataAccess.DAO;
using DataAccess.DAO.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TelegramApi.Client.Clients;
using TelegramApi.Client.Clients.Interfaces;
using TelegramApi.Client.Entities;

namespace TelegramApi.Worker.Installers
{
    public static class ServicesInstaller
    {
        /// <summary>
        ///     Добавляет в serviceCollection сервисы бизнес-логики
        /// </summary>
        public static IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            //data access
            serviceCollection.AddSingleton<IConnectionFactory, NpgConnectionFactory>();
            serviceCollection.AddSingleton<IUpdateDao, UpdateDao>();
            serviceCollection.AddSingleton<IUserDao, UserDao>();
            serviceCollection.AddSingleton<ICityDao, CityDao>();

            //domain
            serviceCollection.AddSingleton<IMapper<Domain.Entities.Update, Update>, UpdateMapper>();

            //application
            serviceCollection.AddSingleton<ICitiesParserService, CitiesParserService>();
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            serviceCollection.AddSingleton<IDialogStateService, DialogStateService>();

            //telegram api
            serviceCollection.AddSingleton<ITelegramBotApiClient, TelegramBotApiClient>();
            serviceCollection.AddHttpClient();
            return serviceCollection;
        }
    }
}

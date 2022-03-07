using DataAccess.ConnectionFactories;
using DataAccess.DAL;
using DataAccess.DAO;
using DataAccess.DAO.Interfaces;
using Domain.Entities;
using Domain.Entities.TelegramApi;
using Domain.Mappers;
using Domain.Mappers.Interfaces;
using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TelegramWorker.Clients;
using TelegramWorker.Clients.Interfaces;
using User = Domain.Entities.User;

namespace TelegramWorker.Installers
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
            serviceCollection.AddSingleton<IDialogStateDao, DialogStateDao>();
            serviceCollection.AddSingleton<ICityDao, CityDao>();

            //domain
            serviceCollection.AddSingleton<IMapper<Update, UpdateDal>, UpdateMapper>();
            serviceCollection.AddSingleton<IMapper<User, UserDal>, DialogStateMapper>();
            serviceCollection.AddSingleton<IMapper<City, CityDal>, CityMapper>();

            serviceCollection.AddSingleton<ICitiesParserService, CitiesParserService>();
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            serviceCollection.AddSingleton<IDialogStateService, DialogStateService>();

            //http clients
            serviceCollection.AddSingleton<ITelegramBotApiClient, TelegramBotApiClient>();
            serviceCollection.AddHttpClient();
            return serviceCollection;
        }
    }
}

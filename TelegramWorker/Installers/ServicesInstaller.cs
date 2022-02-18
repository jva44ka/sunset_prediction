using DataAccess.ConnectionFactories;
using DataAccess.DAL;
using DataAccess.DAO;
using DataAccess.DAO.Interfaces;
using Domain.Entities;
using Domain.Mappers;
using Domain.Mappers.Interfaces;
using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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

            //domain
            serviceCollection.AddSingleton<IMapper<Update, UpdateDal>, UpdateMapper>();
            serviceCollection.AddSingleton<ICitiesParserService, CitiesParserService>();
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            serviceCollection.AddHttpClient();
            return serviceCollection;
        }
    }
}

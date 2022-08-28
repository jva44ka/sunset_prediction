using Application.Mappers;
using Application.Mappers.Interfaces;
using Application.Services;
using Application.Services.EntityServices;
using Application.Services.EntityServices.Interfaces;
using Application.Services.Interfaces;
using DataAccess.ConnectionFactories;
using DataAccess.Dao;
using DataAccess.Dao.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TelegramApi.Client.Clients;
using TelegramApi.Client.Clients.Interfaces;
using TelegramApi.Client.Dtos;
using TelegramApi.Worker.Services;
using TelegramApi.Worker.Services.Interfaces;

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
            serviceCollection.AddSingleton<IChatDao, ChatDao>();

            //entity services
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<ICityService, CityService>();
            serviceCollection.AddSingleton<IChatService, ChatService>();

            //application
            serviceCollection.AddSingleton<IMapper<Domain.Entities.Update, UpdateDto>, UpdateMapper>();
            serviceCollection.AddSingleton<ICitiesStoreService, CitiesStoreService>();
            serviceCollection.AddSingleton<IUpdateHandleService, UpdateHandleService>();
            serviceCollection.AddSingleton<IChatStateService, ChatStateService>();
            serviceCollection.AddSingleton<IAnswerService, AnswerService>();
            serviceCollection.AddSingleton<IChatStateFactory, ChatStateFactory>();

            //telegram api
            serviceCollection.AddSingleton<ITelegramBotApiClient, TelegramBotApiClient>();
            serviceCollection.AddSingleton<ITelegramRequesterService, TelegramRequesterService>();
            serviceCollection.AddHttpClient();
            return serviceCollection;
        }
    }
}

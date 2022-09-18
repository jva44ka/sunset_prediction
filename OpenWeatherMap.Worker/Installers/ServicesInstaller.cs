using Application.Mappers;
using Application.Mappers.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using DataAccess.ConnectionFactories;
using DataAccess.Dao;
using DataAccess.Dao.Interfaces;
using OpenWeatherMap.Client.Clients;
using OpenWeatherMap.Client.Clients.Interfaces;
using TelegramApi.Client.Dtos;

namespace OpenWeatherMap.Worker.Installers;

public static class ServicesInstaller
{
    /// <summary>
    ///     Добавляет в serviceCollection сервисы бизнес-логики
    /// </summary>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        //data access
        serviceCollection.AddSingleton<IConnectionFactory, NpgConnectionFactory>();
        serviceCollection.AddSingleton<IUpdateDao, UpdateDao>();
        serviceCollection.AddSingleton<IUserDao, UserDao>();
        serviceCollection.AddSingleton<ICityDao, CityDao>();

        //domain
        serviceCollection.AddSingleton<IMapper<Domain.Entities.Update, UpdateDto>, UpdateMapper>();

        //application
        serviceCollection.AddSingleton<ICitiesStoreService, CitiesStoreService>();
        serviceCollection.AddSingleton<IUpdateHandleService, UpdateHandleService>();
        serviceCollection.AddSingleton<IChatStateService, ChatStateService>();

        //telegram api
        serviceCollection.AddSingleton<IForecastClient, ForecastClient>();
        serviceCollection.AddSingleton<IGeocodingClient, GeocodingClient>();
        serviceCollection.AddHttpClient();
        return serviceCollection;
    }
}
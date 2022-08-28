using OpenWeatherMap.Client.Dtos;

namespace OpenWeatherMap.Client.Clients.Interfaces;

/// <summary>
///     Клиент для получения прогнозов погоды
/// </summary>
public interface IForecastClient
{
    /// <summary>
    ///     Прогноз на 5 дней доступен в любой точке земного шара.
    ///     Он включает в себя данные прогноза погоды с 3-часовым шагом.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="forecastsCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>Returns <see cref="HourlyForecastResponse"/></remarks>
    Task<HttpResponseMessage> GetHourly(
        double latitude,
        double longitude,
        int forecastsCount,
        CancellationToken cancellationToken = default);
}
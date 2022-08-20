using OpenWeatherMap.Client.Dtos;

namespace OpenWeatherMap.Client.Clients.Interfaces
{
    /// <summary>
    ///     Клиент для получения прогнозов погоды
    /// </summary>
    public interface IForecastClient
    {
        /// <summary>
        ///     Прогноз на 5 дней доступен в любой точке земного шара.
        ///     Он включает в себя данные прогноза погоды с 3-часовым шагом.
        /// </summary>
        Task<HourlyForecastResponse> GetHourly(CancellationToken stoppingToken);
    }
}
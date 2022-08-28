namespace OpenWeatherMap.Client.Clients.Interfaces;

/// <summary>
///     Клиент для поиска местоположений при работе с географическими названиями и координатами.
/// </summary>
public interface IGeocodingClient
{

    /// <summary>
    ///     Поиск координат по названию города и коду страны
    /// </summary>
    /// <param name="cityName"></param>
    /// <param name="countryCode"></param>
    /// <param name="limit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>Returns CoordinatesOfLocationResponseItem[]</remarks>
    Task<HttpResponseMessage> FindCoordinatesByLocationName(
        string cityName,
        string countryCode,
        int limit = 1,
        CancellationToken cancellationToken = default);
}
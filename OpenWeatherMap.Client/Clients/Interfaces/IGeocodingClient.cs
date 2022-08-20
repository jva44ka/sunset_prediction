using OpenWeatherMap.Client.Dtos;

namespace OpenWeatherMap.Client.Clients.Interfaces;

/// <summary>
///     Клиент для поиска местоположений при работе с географическими названиями и координатами.
/// </summary>
public interface IGeocodingClient
{
    /// <summary>
    ///     Поиск координат по названию города и коду страны
    /// </summary>
    Task<CoordinatesOfLocationResponseItem[]> FindCoordinatesByLocationName(CancellationToken stoppingToken);
}
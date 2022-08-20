using OpenWeatherMap.Client.Clients.Interfaces;
using OpenWeatherMap.Client.Dtos;

namespace OpenWeatherMap.Client.Clients;

public class GeocodingClient : IGeocodingClient
{
    public Task<CoordinatesOfLocationResponseItem[]> FindCoordinatesByLocationName(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
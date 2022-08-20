using Microsoft.Extensions.Options;
using OpenWeatherMap.Client.Clients.Interfaces;
using OpenWeatherMap.Client.Dtos;
using OpenWeatherMap.Client.Settings;

namespace OpenWeatherMap.Client.Clients;

public class ForecastClient : IForecastClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenWeatherMapApiSettings _openWeatherMapApiSettings;

    public ForecastClient(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenWeatherMapApiSettings> openWeatherMapApiSettingsOptions)
    {
        _httpClientFactory = httpClientFactory;
        _openWeatherMapApiSettings = openWeatherMapApiSettingsOptions.Value;
    }

    public Task<HourlyForecastResponse> GetHourly(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
using Microsoft.Extensions.Options;
using OpenWeatherMap.Client.Clients.Interfaces;
using OpenWeatherMap.Client.Dtos;
using OpenWeatherMap.Client.Settings;

namespace OpenWeatherMap.Client.Clients;

public class ForecastClient : IForecastClient
{
    private const string GetHourlyEndpoint = "/data/2.5/forecast?lat={lat}&lon={lon}&cnt={cnt}&appid={appid}";
    private const string LatitudeParamName = "lat";
    private const string LongitudeParamName = "lon";
    private const string ForecastsCountParamName = "cnt";
    private const string ApiTokenParamName = "appid";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenWeatherMapApiSettings _openWeatherMapApiSettings;

    private string GetHourlyUrl { get; set; }

    public ForecastClient(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenWeatherMapApiSettings> openWeatherMapApiSettingsOptions)
    {
        _httpClientFactory = httpClientFactory;
        _openWeatherMapApiSettings = openWeatherMapApiSettingsOptions.Value;

        GetHourlyUrl = _openWeatherMapApiSettings.Host + GetHourlyEndpoint;
    }

    public async Task<HourlyForecastResponse> GetHourly(
        double latitude, 
        double longitude, 
        int forecastsCount, 
        CancellationToken stoppingToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var getHourlyUrl = GetHourlyUrl.Replace(LatitudeParamName, latitude.ToString())
                                       .Replace(LongitudeParamName, longitude.ToString())
                                       .Replace(ForecastsCountParamName, forecastsCount.ToString())
                                       .Replace(ApiTokenParamName, _openWeatherMapApiSettings.ApiToken);

        var response = await httpClient.GetAsync(getHourlyUrl, stoppingToken);
    }
}
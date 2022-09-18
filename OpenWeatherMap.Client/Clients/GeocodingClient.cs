using OpenWeatherMap.Client.Clients.Interfaces;
using OpenWeatherMap.Client.Settings;
using Microsoft.Extensions.Options;

namespace OpenWeatherMap.Client.Clients;

public class GeocodingClient : IGeocodingClient
{
    private const string FindCoordinatesByLocationNameEndpoint = "/geo/1.0/direct?q={city name},{country code}&limit={limit}&appid={appid}";
    private const string CityNameParamName = "{city name}";
    private const string CountryCodeParamName = "{country code}";
    private const string LimitParamName = "{limit}";
    private const string ApiTokenParamName = "{appid}";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenWeatherMapApiSettings _openWeatherMapApiSettings;

    private string FindCoordinatesByLocationNameUrl { get; set; }

    public GeocodingClient(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenWeatherMapApiSettings> openWeatherMapApiSettingsOptions)
    {
        _httpClientFactory = httpClientFactory;
        _openWeatherMapApiSettings = openWeatherMapApiSettingsOptions.Value;

        FindCoordinatesByLocationNameUrl = $"{_openWeatherMapApiSettings.Host}{FindCoordinatesByLocationNameEndpoint}";
    }

    public Task<HttpResponseMessage> FindCoordinatesByLocationName(
        string cityName,
        string countryCode,
        int limit = 1,
        CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = FindCoordinatesByLocationNameUrl.Replace(CityNameParamName, cityName)
                                                  .Replace(CountryCodeParamName, countryCode)
                                                  .Replace(LimitParamName, limit.ToString())
                                                  .Replace(ApiTokenParamName, _openWeatherMapApiSettings.ApiToken);

        return httpClient.GetAsync(url, cancellationToken);
    }
}
using OpenWeatherMap.Client.Clients.Interfaces;
using OpenWeatherMap.Worker.Services.Interfaces;

namespace OpenWeatherMap.Worker.Services;

public class OpenWeatherMapRequesterService : IOpenWeatherMapRequesterService
{
    private readonly IForecastClient _forecastClient;
    private readonly IGeocodingClient _geocodingClient;
    private readonly ILogger<IOpenWeatherMapRequesterService> _logger;

    public OpenWeatherMapRequesterService(
        IForecastClient forecastClient,
        IGeocodingClient geocodingClient,
        ILogger<OpenWeatherMapRequesterService> logger)
    {
        _forecastClient = forecastClient;
        _geocodingClient = geocodingClient;
        _logger = logger;
    }
        
    public async Task RequestWeatherCasts(CancellationToken cancellationToken = default)
    {
        var response = new HttpResponseMessage();

        if (response.IsSuccessStatusCode)
        {
            
        }
        else
        {
            _logger.LogError($"Error on forecasts request\t:{DateTime.UtcNow}");
        }
    } 
}
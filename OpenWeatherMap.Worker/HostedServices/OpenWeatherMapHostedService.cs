using Microsoft.Extensions.Options;
using OpenWeatherMap.Worker.Services;
using OpenWeatherMap.Worker.Services.Interfaces;
using OpenWeatherMap.Worker.Settings;

namespace OpenWeatherMap.Worker.HostedServices;

public class OpenWeatherMapHostedService : BackgroundService
{
    private readonly IOpenWeatherMapRequesterService _openWeatherMapRequesterService;
    private readonly OpenWeatherMapWorkerSettings _openWeatherMapWorkerSettings;
    private readonly ILogger<OpenWeatherMapHostedService> _logger;

    public OpenWeatherMapHostedService(
        OpenWeatherMapRequesterService openWeatherMapRequesterService,
        IOptions<OpenWeatherMapWorkerSettings> openWeatherMapWorkerSettingsOptions,
        ILogger<OpenWeatherMapHostedService> logger)
    {
        _openWeatherMapRequesterService = openWeatherMapRequesterService;
        _openWeatherMapWorkerSettings = openWeatherMapWorkerSettingsOptions.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"OpenWeatherMapHostedService: started at: {DateTimeOffset.UtcNow}");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"OpenWeatherMapHostedService: running at: {DateTimeOffset.UtcNow}");

            try
            {
                await _openWeatherMapRequesterService
                    .RequestWeatherCasts(
                        stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
            }

            _logger.LogInformation($"OpenWeatherMapHostedService: sleep for {_openWeatherMapWorkerSettings.ServiceSleepIntervalHours} hours...");
            await Task
                .Delay(
                    TimeSpan.FromHours(_openWeatherMapWorkerSettings.ServiceSleepIntervalHours), 
                    stoppingToken);
        }

        _logger.LogInformation($"OpenWeatherMapHostedService: stopped at: {DateTimeOffset.UtcNow}");
    }
}
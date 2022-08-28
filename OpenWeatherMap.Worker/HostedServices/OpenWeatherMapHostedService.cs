namespace OpenWeatherMap.Worker.HostedServices;

public class OpenWeatherMapHostedService : BackgroundService
{
    private readonly ILogger<OpenWeatherMapHostedService> _logger;

    public OpenWeatherMapHostedService(ILogger<OpenWeatherMapHostedService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("OpenWeatherMapHostedService running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
namespace OpenWeatherMap.Worker.Services.Interfaces;

public interface IOpenWeatherMapRequesterService
{
    Task RequestWeatherCasts(CancellationToken cancellationToken = default);
}
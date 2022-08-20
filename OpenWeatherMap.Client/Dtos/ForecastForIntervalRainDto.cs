using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalRainDto
{
    /// <summary>
    ///     Rain volume for last 3 hours, mm
    /// </summary>
    [JsonProperty("3h")]
    public double Last3HoursVolume { get; set; }
}
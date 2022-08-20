using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalSnowDto
{
    /// <summary>
    ///     Snow volume for last 3 hours
    /// </summary>
    [JsonProperty("3h")]
    public double Last3HoursVolume { get; set; }
}
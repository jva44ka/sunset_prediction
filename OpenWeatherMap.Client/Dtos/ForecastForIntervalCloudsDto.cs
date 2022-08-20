using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalCloudsDto
{
    /// <summary>
    ///     Cloudiness, %
    /// </summary>
    [JsonProperty("all")]
    public int All { get; set; }
}
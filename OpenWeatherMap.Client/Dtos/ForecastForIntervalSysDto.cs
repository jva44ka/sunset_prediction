using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalSysDto
{
    /// <summary>
    ///     Part of the day (n - night, d - day)
    ///     TODO: To enum
    /// </summary>
    [JsonProperty("pod")]
    public char Pod { get; set; }
}
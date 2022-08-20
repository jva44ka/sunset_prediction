using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalCityDto
{
    /// <summary>
    ///     City ID
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///     City name
    /// </summary>
    [JsonProperty("id")]
    public string Name { get; set; } = default!;
    
    /// <summary>
    ///     City name
    /// </summary>
    [JsonProperty("coord")]
    public ForecastForIntervalCityCoordDto Coordinates { get; set; } = default!;
}
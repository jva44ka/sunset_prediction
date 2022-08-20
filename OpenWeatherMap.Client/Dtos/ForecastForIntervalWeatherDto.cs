using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalWeatherDto
{
    /// <summary>
    ///     Weather condition id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///     Group of weather parameters (Rain, Snow, Extreme etc.)
    ///     TODO: To enum
    /// </summary>
    [JsonProperty("main")]
    public string Main { get; set; } = default!;

    /// <summary>
    ///     Group of weather parameters (Rain, Snow, Extreme etc.)
    ///     TODO: To enum
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = default!;

    /// <summary>
    ///     Weather icon id
    /// </summary>
    [JsonProperty("icon")]
    public string Icon { get; set; } = default!;
}
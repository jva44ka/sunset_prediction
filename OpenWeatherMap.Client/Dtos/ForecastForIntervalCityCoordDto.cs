using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalCityCoordDto
{
    /// <summary>
    ///     City geo location, latitude
    /// </summary>
    [JsonProperty("lat")]
    public string Latitude { get; set; } = default!;

    /// <summary>
    ///     City geo location, longitude
    /// </summary>
    [JsonProperty("lon")]
    public string Longitude { get; set; } = default!;

    /// <summary>
    ///     Country code (GB, JP etc.)
    /// </summary>
    [JsonProperty("country")]
    public string CountryCode { get; set; } = default!;

    /// <summary>
    ///     Population
    /// </summary>
    [JsonProperty("population")]
    public int Population { get; set; }

    /// <summary>
    ///     Shift in seconds from UTC
    /// </summary>
    [JsonProperty("timezone")]
    public int Timezone { get; set; }

    /// <summary>
    ///     Sunrise time, Unix, UTC
    /// </summary>
    [JsonProperty("sunrise")]
    public long SunriseTimeUnix { get; set; }
    
    /// <summary>
    ///     Sunrise time, Unix, UTC
    /// </summary>
    [JsonProperty("sunset")]
    public long SunsetTimeUnix { get; set; }
}
using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalDto
{
    /// <summary>
    ///     Time of data forecasted, unix, UTC
    /// </summary>
    [JsonProperty("dt")]
    public long DateTimeUnix { get; set; }

    [JsonProperty("main")] 
    public ForecastForIntervalMainDto Main { get; set; } = default!;
    
    [JsonProperty("weather")] 
    public ForecastForIntervalWeatherDto Weather { get; set; } = default!;
    
    [JsonProperty("clouds")] 
    public ForecastForIntervalCloudsDto Clouds { get; set; } = default!;
    
    [JsonProperty("wind")] 
    public ForecastForIntervalWindDto Wind { get; set; } = default!;

    /// <summary>
    ///     Average visibility, metres. The maximum value of the visibility is 10km
    /// </summary>
    [JsonProperty("visibility")] 
    public int Visibility { get; set; }

    /// <summary>
    ///     Probability of precipitation.
    ///     The values of the parameter vary between 0 and 1, where 0 is equal to 0%, 1 is equal to 100%
    /// </summary>
    [JsonProperty("pop")] 
    public double Pop { get; set; }

    [JsonProperty("rain")]
    public ForecastForIntervalRainDto? Rain { get; set; }
    
    [JsonProperty("snow")]
    public ForecastForIntervalSnowDto? Snow { get; set; }
    
    [JsonProperty("sys")]
    public ForecastForIntervalSysDto Sys { get; set; } = default!;

    /// <summary>
    ///     Time of data forecasted, ISO, UTC
    /// </summary>
    [JsonProperty("dt_txt")]
    public string DateTimeText { get; set; } = default!;
}
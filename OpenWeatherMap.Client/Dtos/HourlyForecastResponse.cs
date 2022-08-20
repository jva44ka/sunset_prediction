using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class HourlyForecastResponse
{
    [JsonProperty("cod")] 
    public string Code { get; set; } = default!;
    
    [JsonProperty("message")] 
    public string Message { get; set; } = default!;

    /// <summary>
    ///     A number of timestamps returned in the API response
    /// </summary>
    [JsonProperty("cnt")] 
    public string Count { get; set; } = default!;

    [JsonProperty("list")]
    public ForecastForIntervalDto[] List { get; set; } = default!;
    
    [JsonProperty("city")]
    public ForecastForIntervalCityDto City { get; set; } = default!;
}
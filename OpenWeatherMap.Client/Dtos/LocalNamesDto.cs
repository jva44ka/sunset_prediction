using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class LocalNamesDto
{
    [JsonProperty("feature_name")]
    public LocalNamesDto FeatureNameDto { get; set; } = default!;
    
    [JsonProperty("en")]
    public LocalNamesDto En { get; set; } = default!;
    [JsonProperty("ru")]
    public LocalNamesDto Ru { get; set; } = default!;
}
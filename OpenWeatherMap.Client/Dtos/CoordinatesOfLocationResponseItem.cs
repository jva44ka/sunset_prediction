using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class CoordinatesOfLocationResponseItem
{
    /// <summary>
    ///     Name of the found location
    /// </summary>
    [JsonProperty("name")] 
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Name of the found location in different languages.
    ///     The list of names can be different for different locations
    /// </summary>
    [JsonProperty("local_names")] 
    public LocalNamesDto LocalNames { get; set; } = default!;

    /// <summary>
    ///     Geographical coordinates of the found location (latitude)
    /// </summary>
    [JsonProperty("lat")] 
    public double Latitude { get; set; }

    /// <summary>
    ///     Geographical coordinates of the found location (longitude)
    /// </summary>
    [JsonProperty("lon")] 
    public double Longitude { get; set; }

    /// <summary>
    ///     Country of the found location
    /// </summary>
    [JsonProperty("country")] 
    public string Country { get; set; } = default!;

    /// <summary>
    ///     State of the found location
    /// </summary>
    [JsonProperty("state")]
    public string? State { get; set; }
}
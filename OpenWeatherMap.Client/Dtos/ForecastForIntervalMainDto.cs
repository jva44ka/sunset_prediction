using Newtonsoft.Json;

namespace OpenWeatherMap.Client.Dtos;

public class ForecastForIntervalMainDto
{
    /// <summary>
    ///     Temperature. Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
    /// </summary>
    [JsonProperty("temp")]
    public double Temperature { get; set; }

    /// <summary>
    ///     This temperature parameter accounts for the human perception of weather.
    ///     Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
    /// </summary>
    [JsonProperty("feels_like")]
    public double FeelsLike { get; set; }

    /// <summary>
    ///     Minimum temperature at the moment of calculation.
    ///     This is minimal forecasted temperature (within large megalopolises and urban areas),
    ///     use this parameter optionally.
    ///     Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
    /// </summary>
    [JsonProperty("temp_min")]
    public double TemperatureMin { get; set; }
    
    /// <summary>
    ///     Minimum temperature at the moment of calculation.
    ///     This is minimal forecasted temperature (within large megalopolises and urban areas),
    ///     use this parameter optionally.
    ///     Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
    /// </summary>
    [JsonProperty("temp_max")]
    public double TemperatureMax { get; set; }

    /// <summary>
    ///     Atmospheric pressure on the sea level by default, hPa
    /// </summary>
    [JsonProperty("pressure")]
    public int Pressure { get; set; }

    /// <summary>
    ///     Atmospheric pressure on the sea level, hPa
    /// </summary>
    [JsonProperty("sea_level")]
    public int SeaLevel { get; set; }

    /// <summary>
    ///     Atmospheric pressure on the ground level, hPa
    /// </summary>
    [JsonProperty("grnd_level")]
    public int GroundLevel { get; set; }

    /// <summary>
    ///     Humidity, %
    /// </summary>
    [JsonProperty("humidity")]
    public int Humidity { get; set; }

    [JsonProperty("temp_kf")]
    public double TemperatureKf { get; set; }
}
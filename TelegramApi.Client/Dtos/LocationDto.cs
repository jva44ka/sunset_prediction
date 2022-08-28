namespace TelegramApi.Client.Dtos;

/// <summary>
///     Этот объект представляет точку на карте.
/// </summary>
public class LocationDto
{
    /// <summary>
    ///     Долгота, заданная отправителем
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    ///     Широта, заданная отправителем
    /// </summary>
    public double Latitude { get; set; }
}
namespace TelegramApi.Client.Entities
{
    /// <summary>
    ///     Этот объект представляет точку на карте.
    /// </summary>
    public class Location
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
}
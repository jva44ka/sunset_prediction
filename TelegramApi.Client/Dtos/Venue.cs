using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет объект на карте.
    /// </summary>
    public class Venue
    {
        /// <summary>
        ///     Координаты объекта
        /// </summary>
        public Location Location { get; set; } = default!;

        /// <summary>
        ///     Название объекта
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// 	Адрес объекта
        /// </summary>
        public string Address { get; set; } = default!;

        /// <summary>
        ///     Опционально. Идентификатор объекта в Foursquare
        /// </summary>
        [JsonProperty("foursquare_id")]
        public string? FoursquareId { get; set; }
    }
}
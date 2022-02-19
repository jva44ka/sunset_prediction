using Newtonsoft.Json;

namespace Domain.Entities.TelegramApi
{
    /// <summary>
    ///     Этот объект представляет объект на карте.
    /// </summary>
    public class Venue
    {
        /// <summary>
        ///     Координаты объекта
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        ///     Название объекта
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 	Адрес объекта
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Опционально. Идентификатор объекта в Foursquare
        /// </summary>
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }
    }
}
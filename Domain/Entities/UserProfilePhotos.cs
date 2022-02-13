using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект содержит фотографии профиля пользователя.
    /// </summary>
    public class UserProfilePhotos
    {
        /// <summary>
        ///     Общее число доступных фотографий профиля
        /// </summary>
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 	Запрошенные изображения, каждое в 4 разных размерах.
        /// </summary>
        public PhotoSize[] Photos { get; set; }
    }
}
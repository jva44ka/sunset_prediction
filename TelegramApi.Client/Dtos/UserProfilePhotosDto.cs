using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект содержит фотографии профиля пользователя.
    /// </summary>
    public class UserProfilePhotosDto
    {
        /// <summary>
        ///     Общее число доступных фотографий профиля
        /// </summary>
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 	Запрошенные изображения, каждое в 4 разных размерах.
        /// </summary>
        public PhotoSizeDto[] Photos { get; set; } = default!;
    }
}
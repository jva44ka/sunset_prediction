using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет бота или пользователя Telegram.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        ///     Уникальный идентификатор пользователя или бота
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Имя бота или пользователя
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; } = default!;

        /// <summary>
        ///     Опционально. Фамилия бота или пользователя
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; } = default!;

        /// <summary>
        ///     Опционально. Username пользователя или бота
        /// </summary>
        public string Username { get; set; } = default!;
    }
}
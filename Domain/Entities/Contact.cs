using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет контакт с номером телефона.
    /// </summary>
    public class Contact
    {
        /// <summary>
        ///     Номер телефона
        /// </summary>
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        ///     Имя
        /// </summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        ///     Опционально. Фамилия
        /// </summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        ///     Опционально. Идентификатор пользователя в Telegram
        /// </summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}
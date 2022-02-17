using Newtonsoft.Json;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет одну кнопку в клавиатуре ответа.
    ///     Для обычных текстовых кнопок этот объект может быть заменён на строку, содержащую текст на кнопке.
    /// </summary>
    public class KeyboardButton
    {
        /// <summary>
        ///     Текст на кнопке. Если ни одно из опциональных полей не использовано, то при нажатии на кнопку
        ///     этот текст будет отправлен боту как простое сообщение.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Опционально. Если значение True, то при нажатии на кнопку боту отправится контакт пользователя
        ///     с его номером телефона. Доступно только в диалогах с ботом.
        /// </summary>
        [JsonProperty("request_contact")]
        public bool RequestContact { get; set; }

        /// <summary>
        ///     Опционально. Если значение True, то при нажатии на кнопку боту отправится местоположение пользователя.
        ///     Доступно только в диалогах с ботом.
        /// </summary>
        [JsonProperty("request_location")]
        public bool RequestLocation { get; set; }
    }
}
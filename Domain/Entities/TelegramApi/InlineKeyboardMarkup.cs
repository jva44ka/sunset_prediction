using Newtonsoft.Json;

namespace Domain.Entities.TelegramApi
{
    /// <summary>
    ///     Этот объект представляет встроенную клавиатуру, которая появляется под соответствующим сообщением.
    /// </summary>
    public class InlineKeyboardMarkup
    {
        /// <summary>
        ///     Массив строк, каждая из которых является массивом объектов InlineKeyboardButton.
        /// </summary>
        [JsonProperty("inline_keyboard")]
        public InlineKeyboardButton[] inline_keyboard { get; set; }
    }
}
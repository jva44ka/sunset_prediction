using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет встроенную клавиатуру, которая появляется под соответствующим сообщением.
    /// </summary>
    public class InlineKeyboardMarkup
    {
        /// <summary>
        ///     Массив строк, каждая из которых является массивом объектов InlineKeyboardButton.
        /// </summary>
        [JsonPropertyName("inline_keyboard")]
        public InlineKeyboardButton[] inline_keyboard { get; set; }
    }
}
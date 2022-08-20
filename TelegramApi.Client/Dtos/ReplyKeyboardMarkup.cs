using System.Linq;
using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет клавиатуру с опциями ответа (см. описание ботов).
    /// </summary>
    public class ReplyKeyboardMarkup
    {
        /// <summary>
        ///     Массив рядов кнопок, каждый из которых является массивом объектов KeyboardButton
        /// </summary>
        [JsonProperty("keyboard")]
        public KeyboardButton[][] Keyboard { get; set; } = default!;

        /// <summary>
        ///     Опционально. Указывает клиенту подогнать высоту клавиатуры под количество кнопок
        ///     (сделать её меньше, если кнопок мало). По умолчанию False, то есть клавиатура всегда такого же размера,
        ///     как и стандартная клавиатура устройства.
        /// </summary>
        [JsonProperty("resize_keyboard")]
        public bool? ResizeKeyboard { get; set; }

        /// <summary>
        ///     Опционально. Указывает клиенту скрыть клавиатуру после использования (после нажатия на кнопку).
        ///     Её по-прежнему можно будет открыть через иконку в поле ввода сообщения. По умолчанию False.
        /// </summary>
        [JsonProperty("one_time_keyboard")]
        public bool? OneTimeKeyboard { get; set; }

        /// <summary>
        ///     Опционально. Этот параметр нужен, чтобы показывать клавиатуру только определённым пользователям. Цели:
        ///         1) пользователи, которые были @упомянуты в поле text объекта Message;
        ///         2) если сообщения бота является ответом (содержит поле reply_to_message_id), авторы этого сообщения.
        ///
        ///     Пример: Пользователь отправляет запрос на смену языка бота. Бот отправляет клавиатуру со списком языков,
        ///     видимую только этому пользователю.
        /// </summary>
        [JsonProperty("selective")]
        public bool? Selective { get; set; }

        public static ReplyKeyboardMarkup CreateFromButtonTexts(params string[] buttonText)
        {
            var keyboard = new ReplyKeyboardMarkup();
            var keyboardRow = buttonText.Select(text => new KeyboardButton
            {
                Text = text
            })
                                        .ToArray();
            keyboard.Keyboard = new KeyboardButton[][] { keyboardRow };
            keyboard.OneTimeKeyboard = true;
            return keyboard;
        }
    }
}
﻿using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет одну кнопку встроенной клавиатуры.
    ///     Вы обязательно должны задействовать ровно одно опциональное поле.
    /// </summary>
    public class InlineKeyboardButton
    {
        /// <summary>
        ///     Текст на кнопке
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Опционально. URL, который откроется при нажатии на кнопку
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Опционально. Данные, которые будут отправлены в callback_query при нажатии на кнопку
        /// </summary>
        [JsonPropertyName("callback_data")]
        public string CallbackData { get; set; }

        /// <summary>
        ///     Опционально. Если этот параметр задан, то при нажатии на кнопку приложение предложит
        ///     пользователю выбрать любой чат, откроет его и вставит в поле ввода сообщения юзернейм бота
        ///     и определённый запрос для встроенного режима. Если отправлять пустое поле, то будет
        ///     вставлен только юзернейм бота.
        ///
        ///     Примечание: это нужно для того, чтобы быстро переключаться между диалогом с ботом и
        ///     встроенным режимом с этим же ботом.Особенно полезно в сочетаниями с действиями switch_pm…
        ///     – в этом случае пользователь вернётся в исходный чат автоматически, без ручного выбора из списка.
        /// </summary>
        [JsonPropertyName("switch_inline_query")]
        public string SwitchInlineQuery { get; set; }
        
        /// <summary>
        ///     Опционально. If set, pressing the button will insert the bot‘s username and the specified
        ///     inline query in the current chat's input field. Can be empty, in which case only the bot’s
        ///     username will be inserted.
        /// </summary>
        [JsonPropertyName("switch_inline_query_current_chat")]
        public string SwitchInlineQueryCurrentChat { get; set; }
    }
}
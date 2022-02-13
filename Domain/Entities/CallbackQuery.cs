﻿using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет входящий запрос обратной связи от инлайн-кнопки с заданным callback_data.
    ///     Если кнопка, создавшая этот запрос, была привязана к сообщению, то в запросе будет присутствовать поле message.
    ///     Если кнопка была показана в сообщении, отправленном при помощи встроенного режима, в запросе будет присутствовать
    ///     поле inline_message_id.
    /// </summary>
    public class CallbackQuery
    {
        /// <summary>
        ///     Уникальный идентификатор запроса
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Отправитель
        /// </summary>
        public User From { get; set; }

        /// <summary>
        ///     Опционально. Сообщение, к которому была привязана вызвавшая запрос кнопка.
        ///     Обратите внимание: если сообщение слишком старое, содержание сообщения и дата отправки будут недоступны.
        /// </summary>
        public Message Message { get; set; }

        /// <summary>
        ///     Опционально. Идентификатор сообщения, отправленного через вашего бота во встроенном режиме
        /// </summary>
        [JsonPropertyName("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        ///     Данные, связанные с кнопкой. Обратите внимание, что клиенты могут добавлять свои данные в это поле.
        /// </summary>
        public string Data { get; set; }
    }
}
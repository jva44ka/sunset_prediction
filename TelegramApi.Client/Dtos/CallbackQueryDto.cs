using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет входящий запрос обратной связи от инлайн-кнопки с заданным callback_data.
    ///     Если кнопка, создавшая этот запрос, была привязана к сообщению, то в запросе будет присутствовать поле message.
    ///     Если кнопка была показана в сообщении, отправленном при помощи встроенного режима, в запросе будет присутствовать
    ///     поле inline_message_id.
    /// </summary>
    public class CallbackQueryDto
    {
        /// <summary>
        ///     Уникальный идентификатор запроса
        /// </summary>
        public string Id { get; set; } = default!;

        /// <summary>
        ///     Отправитель
        /// </summary>
        public UserDto From { get; set; } = default!;

        /// <summary>
        ///     Опционально. Сообщение, к которому была привязана вызвавшая запрос кнопка.
        ///     Обратите внимание: если сообщение слишком старое, содержание сообщения и дата отправки будут недоступны.
        /// </summary>
        public MessageDto? Message { get; set; }

        /// <summary>
        ///     Опционально. Идентификатор сообщения, отправленного через вашего бота во встроенном режиме
        /// </summary>
        [JsonProperty("inline_message_id")]
        public string? InlineMessageId { get; set; }

        /// <summary>
        ///     Данные, связанные с кнопкой. Обратите внимание, что клиенты могут добавлять свои данные в это поле.
        /// </summary>
        public string? Data { get; set; }
    }
}
using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет из себя входящее обновление. Под обновлением подразумевается действие,
    ///     совершённое с ботом — например, получение сообщения от пользователя.
    ///
    ///     Только один из необязательных параметров может присутствовать в каждом обновлении.
    /// </summary>
    public class UpdateDto
    {
        /// <summary>
        ///     The update‘s unique identifier. Update identifiers start from a certain
        ///     positive number and increase sequentially. This ID becomes especially handy
        ///     if you’re using Webhooks, since it allows you to ignore repeated updates or
        ///     to restore the correct update sequence, should they get out of order.
        /// </summary>
        /// <remarks>update_id</remarks>
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }

        /// <summary>
        ///     Опционально. New incoming message of any kind — text, photo, sticker, etc.
        /// </summary>
        /// <remarks>message</remarks>
        public MessageDto? Message { get; set; }

        /// <summary>
        ///     Опционально. New incoming callback query
        /// </summary>
        /// <remarks>callback_query</remarks>
        [JsonProperty("callback_query")]
        public CallbackQueryDto? CallbackQuery { get; set; }
    }
}

﻿using Domain.Entities.TelegramApi;
using Newtonsoft.Json;

namespace TelegramWorker.DTO
{
    public class TelegramSendMessageRequest
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("reply_markup")]
        public ReplyKeyboardMarkup? ReplyMarkup { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}

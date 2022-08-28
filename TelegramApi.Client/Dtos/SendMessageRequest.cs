using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos;

public class SendMessageRequest
{
    [JsonProperty("chat_id")]
    public long ChatId { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; } = default!;

    [JsonProperty("reply_markup")]
    public ReplyKeyboardMarkupDto? ReplyMarkup { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }
}
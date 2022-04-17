using TelegramApi.Client.Entities;

namespace TelegramApi.Client.DTO
{
    public class TelegramGetUpdatesResult
    {
        public bool Ok { get; set; }
        public Update[] Result { get; set; }
    }
}

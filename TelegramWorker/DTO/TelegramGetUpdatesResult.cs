using Domain.Entities.TelegramApi;

namespace TelegramWorker.DTO
{
    public class TelegramGetUpdatesResult
    {
        public bool Ok { get; set; }
        public Update[] Result { get; set; }
    }
}

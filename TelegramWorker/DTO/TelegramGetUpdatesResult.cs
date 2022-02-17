using Domain.Entities;

namespace TelegramWorker.DTO
{
    public class TelegramGetUpdatesResult
    {
        public bool Ok { get; set; }
        public Update[] Result { get; set; }
    }
}

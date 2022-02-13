using Domain.Entities;

namespace TelegramWorker.Dtos
{
    public class TelegramGetUpdatesResult
    {
        public bool Ok { get; set; }
        public Update[] Result { get; set; }
    }
}

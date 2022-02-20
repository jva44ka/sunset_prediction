using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramWorker.Clients.Interfaces
{
    public interface ITelegramBotApiClient
    {
        /// <summary>
        ///     Получение новых Update
        /// </summary>
        Task<HttpResponseMessage> GetUpdates(int? lastHandledUpdateId, CancellationToken stoppingToken);

        /// <summary>
        ///     Запрос на отправку ботом сообщения в чат
        /// </summary>
        Task<HttpResponseMessage> SendMessage(string messageText, int chatId, CancellationToken stoppingToken);
    }
}
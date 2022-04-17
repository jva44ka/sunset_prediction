using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelegramApi.Client.DTO;

namespace TelegramApi.Client.Clients.Interfaces
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
        Task<HttpResponseMessage> SendMessage(TelegramSendMessageRequest request, CancellationToken stoppingToken);
    }
}
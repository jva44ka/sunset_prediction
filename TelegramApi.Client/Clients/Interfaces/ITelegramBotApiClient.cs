using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelegramApi.Client.Dtos;

namespace TelegramApi.Client.Clients.Interfaces;

public interface ITelegramBotApiClient
{
    /// <summary>
    ///     Получение новых Update
    /// </summary>
    Task<HttpResponseMessage> GetUpdates(long? lastHandledUpdateId, CancellationToken stoppingToken);

    /// <summary>
    ///     Запрос на отправку ботом сообщения в чат
    /// </summary>
    Task<HttpResponseMessage> SendMessage(SendMessageRequest request, CancellationToken stoppingToken);
}
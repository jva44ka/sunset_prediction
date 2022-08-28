using System.Threading;
using System.Threading.Tasks;

namespace TelegramApi.Worker.Services.Interfaces
{
    /// <summary>
    ///     Сервис, запрашивающий и обрабатывающий новые обновления бота
    /// </summary>
    public interface ITelegramRequesterService
    {
        /// <summary>
        ///     Запрос и обработка новых обновлений
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleNewUpdates(CancellationToken cancellationToken);
    }
}
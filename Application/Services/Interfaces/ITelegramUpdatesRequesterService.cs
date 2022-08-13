using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    /// <summary>
    ///     Сервис, запрашивающий и обрабатывающий новые обновления бота
    /// </summary>
    public interface ITelegramUpdatesRequesterService
    {
        /// <summary>
        ///     Запрос и обработка новых обновлений
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleNewUpdates(CancellationToken cancellationToken);
    }
}
using System.Threading.Tasks;
using Application.Services.Dto;
using TelegramApi.Client.Dtos;

namespace Application.Services.Interfaces
{
    /// <summary>
    ///     Сервис-таблица переходов состояний диалога
    /// </summary>
    public interface IDialogStateService
    {
        /// <summary>
        ///     Переход на новое состояние диалога в зависимости
        ///     от присланного сообщения и предыдущего состояния
        /// </summary>
        /// <returns>Сообщение в ответ пользователю</returns>
        Task<TransitionResult> TransitionState(
            int userId,
            MessageDto message);
    }
}
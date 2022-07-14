using System.Threading.Tasks;
using Domain.Entities.Enums;
using TelegramApi.Client.Entities;
using User = Domain.Entities.User;

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
        Task<DialogStateService.TransitionResult> TransitionState(
            User? user,
            Message message);

        /// <summary>
        ///     Создание клавиатуры для конкретного нового состояния диалога
        /// </summary>
        /// <param name="dialogState"></param>
        /// <returns></returns>
        ReplyKeyboardMarkup? BuildKeyboard(DialogState dialogState);
    }
}
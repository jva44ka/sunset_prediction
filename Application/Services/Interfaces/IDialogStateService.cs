using System.Threading.Tasks;
using Domain.Entities.Enums;
using TelegramApi.Client.Entities;
using User = Domain.Entities.User;

namespace Application.Services.Interfaces
{
    public interface IDialogStateService
    {
        Task<DialogStateService.TransitionResult> TransitionState(User currentState,
                                                                  Message message);

        ReplyKeyboardMarkup? BuildeKeyboard(DialogState dialogState);
    }
}
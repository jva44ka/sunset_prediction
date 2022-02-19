using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.TelegramApi;

namespace Domain.Services.Interfaces
{
    public interface IDialogStateService
    {
        Task<string> TransitionState(
            DialogState currentState,
            Message message);
    }
}
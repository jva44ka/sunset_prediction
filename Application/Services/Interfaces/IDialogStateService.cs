using System.Threading.Tasks;
using TelegramApi.Client.Entities;

namespace Application.Services.Interfaces
{
    public interface IDialogStateService
    {
        Task<string> TransitionState(
            Domain.Entities.User currentState,
            Message message);
    }
}
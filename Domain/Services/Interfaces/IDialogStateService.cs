using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.TelegramApi;
using User = Domain.Entities.User;

namespace Domain.Services.Interfaces
{
    public interface IDialogStateService
    {
        Task<string> TransitionState(
            User currentState,
            Message message);
    }
}
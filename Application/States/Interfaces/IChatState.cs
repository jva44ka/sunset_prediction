using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;

namespace Application.States.Interfaces
{
    public interface IChatState
    {
        public Task<TransitionResult> HandleTextMessage();
    }
}

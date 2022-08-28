using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class ProposedInputCityState : IChatState
{
    private readonly ChatContext _chatContext;

    public ProposedInputCityState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<TransitionResult> HandleTextMessage()
    {
        var city = await _chatContext.CityService.GetCityByLowerCaseName(_chatContext.MessageText);
        if (city != null)
        {
            var newState = ChatStateType.ProposedFoundedCity;
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);
            await _chatContext.UserService.UpdateCity(_chatContext.ExistingUser.ExternalId, city.Id);

            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.ProposedCityName,
                NewState = newState,
                AnswerMessageArgs = new[] { city.Address }
            };
        }
        else
        {
            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.CityNameNotFound,
                NewState = _chatContext.ExistingChat.CurrentState
            };
        }
    }
}
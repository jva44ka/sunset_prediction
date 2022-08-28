using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class ProposedFoundedCityState : IChatState
{
    private readonly ChatContext _chatContext;

    public ProposedFoundedCityState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<TransitionResult> HandleTextMessage()
    {
        if (_chatContext.MessageText.Trim().ToLower() == "да")
        {
            var newState = ChatStateType.OfChoosingSubscribeType;
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.ProposedSubscribeTypes,
                NewState = newState
            };
        }
        else
        {
            var newState = ChatStateType.ProposedInputCity;
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);
            await _chatContext.UserService.UpdateCity(_chatContext.ExistingUser.ExternalId, null);

            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.ProposedCityNameWrong,
                NewState = newState
            };
        }
    }
}
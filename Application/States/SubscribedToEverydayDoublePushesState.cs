using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class SubscribedToEverydayDoublePushesState : IChatState
{
    private readonly ChatContext _chatContext;

    public SubscribedToEverydayDoublePushesState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<TransitionResult> HandleTextMessage()
    {
        if (_chatContext.MessageText.Trim().ToLower() == "отписка")
        {
            var newState = ChatStateType.SubscribedTriesToUnsubscribe;
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.UnsubscribeWarning,
                NewState = newState
            };
        }
        else
        {
            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.StaysSubscribed,
                NewState = _chatContext.ExistingChat.CurrentState
            };
        }
    }
}
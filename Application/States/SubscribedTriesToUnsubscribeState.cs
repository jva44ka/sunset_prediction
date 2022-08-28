using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System;
using System.Threading.Tasks;

namespace Application.States;

public class SubscribedTriesToUnsubscribeState : IChatState
{
    private readonly ChatContext _chatContext;

    public SubscribedTriesToUnsubscribeState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        if (_chatContext.MessageText.Trim().ToLower() == "да")
        {
            var newState = ChatStateType.Unsubscribed;
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.Unsubscribed,
                NewState = newState
            };
        }
        else
        {
            var newState = _chatContext.ExistingChat.PreviousState
                           ?? throw new Exception(
                               $"User has not previous state with external id: {_chatContext.ExistingChat.ExternalId}");
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.StaysSubscribed,
                NewState = newState
            };
        }
    }
}
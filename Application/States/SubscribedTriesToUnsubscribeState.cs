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
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();

        if (_chatContext.MessageText!.Trim().ToLower() == "да")
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId, 
                ChatStateType.Unsubscribed);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.Unsubscribed
            };
        }
        else
        {
            var newState = _chatContext.ExistingChat!.PreviousState
                           ?? throw new Exception(
                               $"User has not previous state with external id: {_chatContext.ExistingChat.ExternalId}");
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.StaysSubscribed
            };
        }
    }
}
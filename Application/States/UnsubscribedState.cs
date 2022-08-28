﻿using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class UnsubscribedState : IChatState
{
    private readonly ChatContext _chatContext;

    public UnsubscribedState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        if (_chatContext.MessageText.Trim().ToLower() == "подписка")
        {
            var newState = ChatStateType.UnsubscribedTriesSubscribe;
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.ProposedSubscribeTypes,
                NewState = newState
            };
        }
        else
        {
            return new AnswerDto
            {
                MessageType = AnswerMessageType.Unsubscribed,
                NewState = _chatContext.ExistingChat.CurrentState
            };
        }
    }
}
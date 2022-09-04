using System;
using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using Application.Enums;

namespace Application.States;

public class SubscribedState : IChatState
{
    private readonly ChatContext _chatContext;

    public SubscribedState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();
        _chatContext.ValidateExistingUser();

        var existSubscription = _chatContext.ExistingUser!.SubscribeType!.Value;

        if (_chatContext.MessageText!.Trim().ToLower() == "отписка")
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId,
                ChatStateType.RequestedUnsubscribe);

            switch (existSubscription)
            {
                case (SubscribeType.Sunset | SubscribeType.Lightning):
                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.RequestedUnsubscribeWithSunsetAndLightningSubscribes
                    };
                case (SubscribeType.Sunset):
                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.RequestedUnsubscribeWithSunsetSubscribed
                    };
                case (SubscribeType.Lightning):
                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.RequestedUnsubscribeWithSunsetSubscribed
                    };
                default: 
                    throw new ArgumentOutOfRangeException(
                        nameof(existSubscription));
            }
        }
        else
        {
            switch (existSubscription)
            {
                case (SubscribeType.Sunset | SubscribeType.Lightning):
                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.SubscribedToSunsetAndLightning
                    };
                case (SubscribeType.Sunset):
                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.SubscribedToSunset
                    };
                case (SubscribeType.Lightning):
                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.SubscribedToLightning
                    };
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(existSubscription));
            }
        }
    }
}
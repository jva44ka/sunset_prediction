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

        switch (_chatContext.MessageText!.Trim().ToLower())
        {
            case "отписка":
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
            case "подписка":
                {
                    if (_chatContext.ExistingUser.SubscribeType!.Value.HasFlag(SubscribeType.Sunset | SubscribeType.Lightning))
                    {
                        throw new Exception(
                            $"User {_chatContext.ExistingUser.ExternalId} already exists all subscription types");
                    }

                    await _chatContext.ChatService.UpdateState(
                        _chatContext.ExistingChat!.ExternalId,
                        ChatStateType.RequestedNewSubscribe);

                    switch (existSubscription)
                    {
                        case (SubscribeType.Sunset):
                            return new AnswerDto
                            {
                                MessageType = AnswerMessageType.RequestedNewSubscribeWithSunsetSubscribe
                            };
                        case (SubscribeType.Lightning):
                            return new AnswerDto
                            {
                                MessageType = AnswerMessageType.RequestedNewSubscribeWithLightningSubscribed
                            };
                        default:
                            throw new ArgumentOutOfRangeException(
                                nameof(existSubscription));
                    }
                }

            default:
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
}
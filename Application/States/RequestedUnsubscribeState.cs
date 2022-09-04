using System;
using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using Domain.Extensions;
using Application.Enums;

namespace Application.States;

public class RequestedUnsubscribeState : IChatState
{
    private readonly ChatContext _chatContext;

    public RequestedUnsubscribeState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();
        _chatContext.ValidateExistingUser();

        switch (_chatContext.MessageText!.Trim().ToLower())
        {
            case "закаты":
                {
                    await UnsubscribeFrom(SubscribeType.Sunset);

                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.SubscribedToSunset
                    };
                }

            case "грозы":
                {
                    await UnsubscribeFrom(SubscribeType.Lightning);

                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.Unsubscribed
                    };
                }

            case "от всех":
                {
                    await UnsubscribeFrom(SubscribeType.Sunset | SubscribeType.Lightning);

                    return new AnswerDto
                    {
                        MessageType = AnswerMessageType.SubscribedToSunsetAndLightning
                    };
                }

            default:
                await _chatContext.ChatService.UpdateState(
                    _chatContext.ExistingChat!.ExternalId,
                    _chatContext.ExistingChat.PreviousState!.Value);

                return new AnswerDto
                {
                    MessageType = AnswerMessageType.InputSubscribeNameWrong
                };
        }
    }

    private async Task UnsubscribeFrom(
        SubscribeType subscribeType)
    {
        var isAlreadyUnsubscribed = _chatContext.ExistingUser!.SubscribeType!.Value.HasFlag(subscribeType) == false;
        if (isAlreadyUnsubscribed)
        {
            throw new Exception(
                $"User {_chatContext.ExistingUser!.ExternalId} already subscribed from {subscribeType}");
        }

        var newSubscriptionType = _chatContext.ExistingUser!.SubscribeType.Value.Subtract(subscribeType);
        
        if (newSubscriptionType == SubscribeType.None)
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId,
                ChatStateType.WithoutSubscribe);
        }
        await _chatContext.UserService.UpdateSubscription(
            _chatContext.ExistingChat!.ExternalId,
            newSubscriptionType);
    }
}
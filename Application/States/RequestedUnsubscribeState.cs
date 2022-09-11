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
                    var newSubscription = await UnsubscribeFrom(SubscribeType.Sunset);
                    return GetAnswerMessageType(newSubscription);
                }

            case "грозы":
                {
                    var newSubscription = await UnsubscribeFrom(SubscribeType.Lightning);
                    return GetAnswerMessageType(newSubscription);
                }

            case "от всех":
                {
                    var newSubscription = await UnsubscribeFrom(SubscribeType.Sunset | SubscribeType.Lightning);
                    return GetAnswerMessageType(newSubscription);
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

    private async Task<SubscribeType> UnsubscribeFrom(
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
        else
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId,
                ChatStateType.Subscribed);
        }

        await _chatContext.UserService.UpdateSubscription(
            _chatContext.ExistingChat!.ExternalId,
            newSubscriptionType);

        return newSubscriptionType;
    }

    private AnswerDto GetAnswerMessageType(
        SubscribeType subscribeType)
    {
        switch (subscribeType)
        {
            case (SubscribeType.Sunset):
            {
                return new AnswerDto
                {
                    MessageType = AnswerMessageType.SubscribedToSunset
                };
            }
            case (SubscribeType.Lightning):
            {
                return new AnswerDto
                {
                    MessageType = AnswerMessageType.SubscribedToLightning
                };
            }
            case (SubscribeType.None):
            {
                return new AnswerDto
                {
                    MessageType = AnswerMessageType.Unsubscribed
                };
            }

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(subscribeType));
        }
    }
}
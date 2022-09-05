using System;
using System.Threading.Tasks;
using Application.Enums;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;

namespace Application.States;

public class RequestedSubscribeState : IChatState
{
    private readonly ChatContext _chatContext;

    public RequestedSubscribeState(ChatContext chatContext)
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
                var newSubscribeType = await AddSubscribeType(SubscribeType.Sunset);
                return GetAnswerMessageType(newSubscribeType);
            }

            case "грозы":
            {
                var newSubscribeType = await AddSubscribeType(SubscribeType.Lightning);
                return GetAnswerMessageType(newSubscribeType);
            }

            case "закаты и грозы":
            {
                var newSubscribeType = await AddSubscribeType(SubscribeType.Sunset | SubscribeType.Lightning);
                return GetAnswerMessageType(newSubscribeType);
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
    
    private async Task<SubscribeType> AddSubscribeType(
        SubscribeType newSubscribeType)
    {
        _chatContext.ExistingUser!.SubscribeType ??= SubscribeType.None;
        var isAlreadySubscribed = _chatContext.ExistingUser!.SubscribeType.Value.HasFlag(newSubscribeType);
        if (isAlreadySubscribed)
        {
            throw new Exception(
                $"User {_chatContext.ExistingUser!.ExternalId} already has subscribe {newSubscribeType}");
        }

        var newSubscription = _chatContext.ExistingUser!.SubscribeType.Value | newSubscribeType;

        if (_chatContext.ExistingChat!.CurrentState != ChatStateType.Subscribed)
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId,
                ChatStateType.Subscribed);
        }

        await _chatContext.UserService.UpdateSubscription(
            _chatContext.ExistingChat!.ExternalId,
            newSubscription);

        return newSubscription;
    }
    
    private AnswerDto GetAnswerMessageType(
        SubscribeType subscribeType)
    {
        switch (subscribeType)
        {
            case (SubscribeType.Sunset | SubscribeType.Lightning):
            {
                return new AnswerDto
                {
                    MessageType = AnswerMessageType.SubscribedToSunsetAndLightning
                };
            }
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

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(subscribeType));
        }
    }
}
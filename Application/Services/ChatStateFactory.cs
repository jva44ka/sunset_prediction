using System;
using System.Collections.Generic;
using Application.Services.Interfaces;
using Application.States;
using Application.States.Interfaces;
using Domain.Entities.Enums;

namespace Application.Services;

//TODO: Отрефакторить без использования рефлексии, если возможно
public class ChatStateFactory : IChatStateFactory
{
    private readonly Dictionary<ChatStateType, Type> _chatStatesByTypes =
        new Dictionary<ChatStateType, Type>
        {
            { ChatStateType.ProposedInputCity, typeof(ProposedInputCityState) },
            { ChatStateType.FoundedProposedCity, typeof(FoundedProposedCityState)},
            { ChatStateType.WithoutSubscribe, typeof(WithoutSubscribeState)},
            { ChatStateType.RequestedNewSubscribe, typeof(RequestedSubscribeState)},
            { ChatStateType.Subscribed, typeof(SubscribedState)},
            { ChatStateType.RequestedUnsubscribe, typeof(RequestedUnsubscribeState)}
        };

    public IChatState Create(
        ChatStateType? chatStateType,
        ChatContext chatContext)
    {
        if (chatStateType == null)
        {
            return new WithoutState(chatContext);
        }

        var stateClass = _chatStatesByTypes[chatStateType.Value];
        return (IChatState)Activator.CreateInstance(stateClass, chatContext);
    }
}
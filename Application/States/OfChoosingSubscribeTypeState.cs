using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;

namespace Application.States;

public class OfChoosingSubscribeTypeState : IChatState
{
    private readonly ChatContext _chatContext;

    public OfChoosingSubscribeTypeState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<TransitionResult> HandleTextMessage()
    {
        switch (_chatContext.MessageText.Trim().ToLower())
        {
            case "обычная":
            {
                var newState = ChatStateType.SubscribedToEverydayPushes;
                await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.SubscribedToEverydayPushes,
                    NewState = newState
                };
            }

            case "двойная":
            {
                var newState = ChatStateType.SubscribedToEverydayDoublePushes;
                await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.SubscribedToEverydayDoublePushes,
                    NewState = newState
                };
            }

            default:
                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.InputSubscribeNameWrong,
                    NewState = _chatContext.ExistingChat.CurrentState
                };
        }
    }
}
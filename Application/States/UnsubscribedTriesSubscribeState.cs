using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class UnsubscribedTriesSubscribeState : IChatState
{
    private readonly ChatContext _chatContext;

    public UnsubscribedTriesSubscribeState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    //TODO: Копия OfChoosingSubscribeTypeState
    public async Task<AnswerDto> HandleTextMessage()
    {
        switch (_chatContext.MessageText.Trim().ToLower())
        {
            case "обычная":
            {
                var newState = ChatStateType.SubscribedToEverydayPushes;
                await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

                return new AnswerDto
                {
                    MessageType = AnswerMessageType.SubscribedToEverydayPushes,
                    NewState = newState
                };
            }

            case "двойная":
            {
                var newState = ChatStateType.SubscribedToEverydayDoublePushes;
                await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

                return new AnswerDto
                {
                    MessageType = AnswerMessageType.SubscribedToEverydayDoublePushes,
                    NewState = newState
                };
            }

            default:
                return new AnswerDto
                {
                    MessageType = AnswerMessageType.InputSubscribeNameWrong,
                    NewState = _chatContext.ExistingChat.CurrentState
                };
        }
    }
}
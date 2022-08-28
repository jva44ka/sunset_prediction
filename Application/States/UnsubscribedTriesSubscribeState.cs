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
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();

        switch (_chatContext.MessageText!.Trim().ToLower())
        {
            case "обычная":
            {
                await _chatContext.ChatService.UpdateState(
                    _chatContext.ExistingChat!.ExternalId, 
                    ChatStateType.SubscribedToEverydayPushes);

                return new AnswerDto
                {
                    MessageType = AnswerMessageType.SubscribedToEverydayPushes
                };
            }

            case "двойная":
            {
                await _chatContext.ChatService.UpdateState(
                    _chatContext.ExistingChat!.ExternalId, 
                    ChatStateType.SubscribedToEverydayDoublePushes);

                return new AnswerDto
                {
                    MessageType = AnswerMessageType.SubscribedToEverydayDoublePushes
                };
            }

            default:
                return new AnswerDto
                {
                    MessageType = AnswerMessageType.InputSubscribeNameWrong
                };
        }
    }
}
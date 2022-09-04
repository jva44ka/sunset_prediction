using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;

namespace Application.States;

public class SubscribedSunsetAndLightningState : IChatState
{
    private readonly ChatContext _chatContext;

    public SubscribedSunsetAndLightningState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

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
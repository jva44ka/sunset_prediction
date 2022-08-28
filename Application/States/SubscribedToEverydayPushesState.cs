using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class SubscribedToEverydayPushesState : IChatState
{
    private readonly ChatContext _chatContext;

    public SubscribedToEverydayPushesState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    //TODO: Копия SubscribedToEverydayDoublePushesState
    public async Task<AnswerDto> HandleTextMessage()
    {
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();

        if (_chatContext.MessageText!.Trim().ToLower() == "отписка")
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId, 
                ChatStateType.SubscribedTriesToUnsubscribe);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.UnsubscribeWarning
            };
        }
        else
        {
            return new AnswerDto
            {
                MessageType = AnswerMessageType.StaysSubscribed
            };
        }
    }
}
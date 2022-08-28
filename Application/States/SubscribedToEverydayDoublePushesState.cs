using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class SubscribedToEverydayDoublePushesState : IChatState
{
    private readonly ChatContext _chatContext;

    public SubscribedToEverydayDoublePushesState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        if (_chatContext.MessageText.Trim().ToLower() == "отписка")
        {
            var newState = ChatStateType.SubscribedTriesToUnsubscribe;
            await _chatContext.ChatService.UpdateState(_chatContext.ExistingChat.ExternalId, newState);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.UnsubscribeWarning,
                NewState = newState
            };
        }
        else
        {
            return new AnswerDto
            {
                MessageType = AnswerMessageType.StaysSubscribed,
                NewState = _chatContext.ExistingChat.CurrentState
            };
        }
    }
}
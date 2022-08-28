using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class UnsubscribedState : IChatState
{
    private readonly ChatContext _chatContext;

    public UnsubscribedState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();

        if (_chatContext.MessageText!.Trim().ToLower() == "подписка")
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId, 
                ChatStateType.UnsubscribedTriesSubscribe);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.ProposedSubscribeTypes
            };
        }
        else
        {
            return new AnswerDto
            {
                MessageType = AnswerMessageType.Unsubscribed
            };
        }
    }
}
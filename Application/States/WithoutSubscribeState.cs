using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using Application.Enums;

namespace Application.States;

public class WithoutSubscribeState : IChatState
{
    private readonly ChatContext _chatContext;

    public WithoutSubscribeState(ChatContext chatContext)
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
                ChatStateType.RequestedNewSubscribe);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.RequestedNewSubscribeWithoutSubscribes
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
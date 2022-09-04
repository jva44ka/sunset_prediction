using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;

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

            var availableSubscribes = new[] { "Грозы", "Закаты", "Закаты и грозы" };

            return new AnswerDto
            {
                MessageType = AnswerMessageType.RequestedNewSubscribe,
                MessageArgs = availableSubscribes
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
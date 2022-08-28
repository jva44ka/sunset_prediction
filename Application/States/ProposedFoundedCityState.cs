using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.States;

public class ProposedFoundedCityState : IChatState
{
    private readonly ChatContext _chatContext;

    public ProposedFoundedCityState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();
        _chatContext.ValidateExistingUser();

        if (_chatContext.MessageText!.Trim().ToLower() == "да")
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId, 
                ChatStateType.OfChoosingSubscribeType);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.ProposedSubscribeTypes
            };
        }
        else
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId, 
                ChatStateType.ProposedInputCity);
            await _chatContext.UserService.UpdateCity(
                _chatContext.ExistingUser!.ExternalId, null);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.ProposedCityNameWrong
            };
        }
    }
}
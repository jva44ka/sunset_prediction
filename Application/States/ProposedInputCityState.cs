using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities.Enums;
using System.Threading.Tasks;
using Application.Enums;

namespace Application.States;

public class ProposedInputCityState : IChatState
{
    private readonly ChatContext _chatContext;

    public ProposedInputCityState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        _chatContext.ValidateMessageText();
        _chatContext.ValidateExistingChat();
        _chatContext.ValidateExistingUser();

        var city = await _chatContext.CityService.GetCityByName(_chatContext.MessageText!);
        if (city != null)
        {
            await _chatContext.ChatService.UpdateState(
                _chatContext.ExistingChat!.ExternalId, 
                ChatStateType.FoundedProposedCity);
            await _chatContext.UserService.UpdateCity(
                _chatContext.ExistingUser!.ExternalId, 
                city.Id);

            return new AnswerDto
            {
                MessageType = AnswerMessageType.ProposedFoundedCityName,
                MessageArgs = new[] { city.Address }
            };
        }
        else
        {
            return new AnswerDto
            {
                MessageType = AnswerMessageType.CityNameNotFound
            };
        }
    }
}
using System;
using System.Threading.Tasks;
using Application.Services;
using Application.Services.Dto;
using Application.States.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;
using Application.Enums;

namespace Application.States;

public class WithoutState : IChatState
{
    private readonly ChatContext _chatContext;

    public WithoutState(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    public async Task<AnswerDto> HandleTextMessage()
    {
        var chat = new Chat
        {
            ExternalId = _chatContext.ChatExternalId,
            CurrentState = ChatStateType.ProposedInputCity,
            StateChangedAt = DateTime.UtcNow
        };
        await _chatContext.ChatService.Create(chat);

        var existingChat = await _chatContext.ChatService.GetByExternalId(_chatContext.ChatExternalId);
        var user = new User
        {
            ExternalId = _chatContext.UserDto.Id,
            FirstName = _chatContext.UserDto.FirstName,
            LastName = _chatContext.UserDto.LastName,
            UserName = _chatContext.UserDto.Username,
            ChatId = existingChat.Id
        };
        await _chatContext.UserService.Create(user);

        return new AnswerDto
        {
            MessageType = AnswerMessageType.ProposedInputCity
        };
    }
}
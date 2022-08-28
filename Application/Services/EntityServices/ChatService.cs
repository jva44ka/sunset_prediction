using System;
using System.Threading.Tasks;
using Application.Services.EntityServices.Interfaces;
using DataAccess.Dao.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;

namespace Application.Services.EntityServices;

public class ChatService : IChatService
{
    private readonly IChatDao _chatDao;

    public ChatService(IChatDao chatDao)
    {
        _chatDao = chatDao;
    }

    public Task<Chat?> GetByExternalId(long externalId)
    {
        return _chatDao.GetByExternalId(externalId);
    }

    public async Task<bool> Create(Chat chat)
    {
        var existingChat = await _chatDao.GetByExternalId(chat.ExternalId);
        if (existingChat != null)
        {
            throw new Exception(
                $"Chat is already exists in database with external id: {chat.ExternalId}");
        }

        return await _chatDao.Create(chat);
    }

    public async Task<bool> UpdateState(long externalId, ChatState newState)
    {
        var chat = await _chatDao.GetByExternalId(externalId);

        if (chat == null)
        {
            throw new Exception(
                $"User not found in database by external id: {externalId}");
        }

        var previousState = chat.CurrentState;
        var stateChangeDate = DateTime.UtcNow;

        return await _chatDao.UpdateState(
            chat.Id,
            previousState,
            newState,
            stateChangeDate);
    }
}
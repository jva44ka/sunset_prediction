using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Enums;

namespace DataAccess.Dao.Interfaces;

public interface IChatDao
{
    Task<Chat?> GetByExternalId(long externalId);

    Task<bool> Create(Chat chat);

    Task<bool> UpdateState(
        int chatId,
        ChatStateType previousState,
        ChatStateType currentState,
        DateTime stateChangeDate);
}
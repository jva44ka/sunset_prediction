using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Enums;

namespace Application.Services.EntityServices.Interfaces;

public interface IChatService
{
    Task<Chat?> GetByExternalId(long externalId);

    Task<bool> Create(Chat chat);

    Task<bool> UpdateState(long externalId, ChatStateType newState);
}
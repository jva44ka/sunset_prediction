using Application.States.Interfaces;
using Domain.Entities.Enums;

namespace Application.Services.Interfaces;

public interface IChatStateFactory
{
    IChatState Create(
        ChatStateType? chatStateType,
        ChatContext chatContext);
}
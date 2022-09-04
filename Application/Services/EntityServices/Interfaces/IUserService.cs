using Domain.Entities;
using Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Application.Services.EntityServices.Interfaces;

public interface IUserService
{
    Task<User?> GetByExternalId(long externalId);

    Task<bool> Create(User user);

    Task<bool> UpdateCity(long externalId, int? cityId);

    Task<bool> UpdateSubscription(long externalId, SubscribeType? subscribeType);
}
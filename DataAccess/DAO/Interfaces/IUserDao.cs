using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Enums;

namespace DataAccess.Dao.Interfaces;

public interface IUserDao
{
    Task<User?> GetByExternalId(long externalId);

    Task<bool> Create(User user);

    Task<bool> UpdateCity(
        int userId,
        int? cityId);

    Task<bool> UpdateSubscribeType(
        int userId,
        SubscribeType? subscribeType);
}
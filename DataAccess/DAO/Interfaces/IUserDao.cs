using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Enums;

namespace DataAccess.Dao.Interfaces
{
    public interface IUserDao
    {
        Task<User?> GetByExternalId(long externalId);
        Task<bool> Create(User user);
        Task<bool> UpdateState(
            int userId,
            DialogState previousState,
            DialogState currentState,
            DateTime stateChangeDate);
        Task<bool> UpdateCity(
            int userId,
            int? cityId);
    }
}
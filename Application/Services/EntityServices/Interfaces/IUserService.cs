using Domain.Entities;
using System.Threading.Tasks;
using Domain.Entities.Enums;

namespace Application.Services.EntityServices.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByExternalId(long externalId);

        Task<bool> Create(User user);

        Task<bool> UpdateState(long externalId, DialogState newState);

        Task<bool> UpdateCity(long externalId, int? cityId);
    }
}

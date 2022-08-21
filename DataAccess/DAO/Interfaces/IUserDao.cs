using System.Threading.Tasks;
using Domain.Entities;

namespace DataAccess.Dao.Interfaces
{
    public interface IUserDao
    {
        Task<User?> GetByExternalId(int externalId);
        Task<bool> Create(User user);
        Task<bool> Update(User user);
    }
}
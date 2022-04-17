using System.Threading.Tasks;
using Domain.Entities;

namespace DataAccess.DAO.Interfaces
{
    public interface IUserDao
    {
        Task<User?> GetStateByUserId(int userId);
        Task<bool> Create(User userDal);
        Task<bool> Update(User userDal);
    }
}
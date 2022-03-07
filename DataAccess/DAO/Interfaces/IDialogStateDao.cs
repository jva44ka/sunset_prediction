using System.Threading.Tasks;
using DataAccess.DAL;

namespace DataAccess.DAO.Interfaces
{
    public interface IDialogStateDao
    {
        Task<UserDal?> GetStateByUserId(int userId);
        Task<bool> Create(UserDal userDal);
        Task<bool> Update(UserDal userDal);
    }
}
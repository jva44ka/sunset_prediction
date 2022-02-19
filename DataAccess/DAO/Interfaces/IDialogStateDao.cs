using System.Threading.Tasks;
using DataAccess.DAL;

namespace DataAccess.DAO.Interfaces
{
    public interface IDialogStateDao
    {
        Task<DialogStateDal?> GetStateByUserId(int userId);
        Task<bool> Create(DialogStateDal dialogStateDal);
        Task<bool> Update(DialogStateDal dialogStateDal);
    }
}
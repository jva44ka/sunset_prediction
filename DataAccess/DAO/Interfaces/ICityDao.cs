using System.Threading.Tasks;
using DataAccess.DAL;

namespace DataAccess.DAO.Interfaces
{
    public interface ICityDao
    {
        Task<CityDal?> GetCityById(int id);
        Task<bool> Create(CityDal cityDal);
        Task<bool> Update(CityDal cityDal);
    }
}
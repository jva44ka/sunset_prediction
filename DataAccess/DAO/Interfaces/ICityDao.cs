using System.Threading.Tasks;
using Domain.Entities;

namespace DataAccess.DAO.Interfaces
{
    public interface ICityDao
    {
        Task<City?> GetCityById(int id);
        Task<City?> GetCityByLowerCaseName(string cityName);
        Task<bool> Create(City cityDal);
        Task<bool> Update(City cityDal);
    }
}
using System.Threading.Tasks;
using Domain.Entities;

namespace DataAccess.Dao.Interfaces;

public interface ICityDao
{
    Task<City?> GetCityById(int id);
    Task<City?> GetCityByName(string cityName);
    Task<bool> Create(City city);
    Task<bool> Update(City city);
}
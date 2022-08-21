using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services.EntityServices.Interfaces
{
    public interface ICityService
    {
        public Task<City?> GetCityById(int id);

        public Task<City?> GetCityByLowerCaseName(string cityName);

        public Task<bool> Create(City cityDal);

        public Task<bool> Update(City cityDal);
    }
}

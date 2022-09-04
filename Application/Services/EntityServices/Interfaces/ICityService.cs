using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services.EntityServices.Interfaces;

public interface ICityService
{
    public Task<City?> GetCityById(int id);

    public Task<City?> GetCityByName(string cityName);

    public Task<bool> Create(City city);

    public Task<bool> Update(City city);
}
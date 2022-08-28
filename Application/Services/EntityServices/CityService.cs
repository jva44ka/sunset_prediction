using System.Threading.Tasks;
using Application.Services.EntityServices.Interfaces;
using Application.Services.Interfaces;
using DataAccess.Dao.Interfaces;
using Domain.Entities;

namespace Application.Services.EntityServices;

public class CityService : ICityService
{
    private readonly ICitiesStoreService _citiesStoreService;
    private readonly ICityDao _cityDao;

    public CityService(
        ICitiesStoreService citiesStoreService,
        ICityDao cityDao)
    {
        _citiesStoreService = citiesStoreService;
        _cityDao = cityDao;
    }

    public async Task<City?> GetCityById(int id)
    {
        var cityExistsInDb = true;
        var city = await _cityDao.GetCityById(id);

        if (city == null)
        {
            cityExistsInDb = false;
            city = await _citiesStoreService.FindCity(id);
        }

        if (city == null)
        {
            return null;
        }

        if (cityExistsInDb == false)
        {
            await _cityDao.Create(city);
        }

        return city;
    }

    public async Task<City?> GetCityByLowerCaseName(string cityName)
    {
        var cityLowerCaseName = cityName.Trim().ToLower();

        var cityExistsInDb = true;
        var city = await _cityDao.GetCityByLowerCaseName(cityLowerCaseName);

        if (city == null)
        {
            //TODO: Поменять на cityLowerCaseName для единства
            cityExistsInDb = false;
            city = await _citiesStoreService.FindCity(cityName);
        }

        if (city == null)
        {
            return null;
        }

        if (cityExistsInDb == false)
        {
            await _cityDao.Create(city);
        }

        return city;
    }

    public Task<bool> Create(City city)
    {
        return _cityDao.Create(city);
    }

    public Task<bool> Update(City city)
    {
        return _cityDao.Update(city);
    }
}
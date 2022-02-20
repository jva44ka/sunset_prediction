using DataAccess.DAL;
using Domain.Entities;
using Domain.Entities.TelegramApi;
using Domain.Mappers.Interfaces;

namespace Domain.Mappers
{
    public class CityMapper : IMapper<City, CityDal>
    {
        public City? ToEntity(CityDal? dal)
        {
            if (dal == null)
            {
                return null;
            }

            return new City
            {
                Id = dal.Id,
                CountryCode = dal.CountryCode,
                Address = dal.Address,
                Latitude = dal.Latitude,
                Longitude = dal.Longitude,
                Name = dal.Name,
                UrlName = dal.UrlName
            };
        }

        public CityDal? ToDal(City? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CityDal
            {
                Id = entity.Id,
                CountryCode = entity.CountryCode,
                Address = entity.Address,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Name = entity.Name,
                UrlName = entity.UrlName
            };
        }
    }
}
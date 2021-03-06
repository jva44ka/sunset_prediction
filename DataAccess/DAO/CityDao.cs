using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.DAO.Interfaces;
using Domain.Entities;

namespace DataAccess.DAO
{
    public class CityDao : ICityDao
    {
        private readonly IConnectionFactory _connectionFactory;

        private const string SelectFieldNames = @"
    id              AS  Id,
    name            AS  Name,
    url_name        AS  UrlName,
    address         AS  Address,
    country_code    AS  CountryCode,
    latitude        AS  Latitude,
    longitude       AS  Longitude
";

        public CityDao(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<City?> GetCityById(int id)
        {
            string sql =
                $@"
SELECT
    {SelectFieldNames}
FROM 
    cities
WHERE
    id = @id";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var cityDal = await connection.QueryFirstOrDefaultAsync<City>(sql, new
            {
                id = id
            });
            return cityDal;
        }
        
        public async Task<City?> GetCityByLowerCaseName(string cityName)
        {
            string sql =
                $@"
SELECT
    {SelectFieldNames}
FROM 
    cities
WHERE
    LOWER(cities.name) LIKE CONCAT('%', @cityName, '%')";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var cityDal = await connection.QueryFirstOrDefaultAsync<City>(sql, new
            {
                cityName = cityName
            });
            return cityDal;
        }
        
        public async Task<bool> Create(City cityDal)
        {
            string sql =
                @"
INSERT INTO 
    cities
VALUES (
    @id,
    @name,
    @url_name,
    @address,
    @country_code,
    @latitude,
    @longitude
)";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                id = cityDal.Id,
                name = cityDal.Name,
                url_name = cityDal.UrlName,
                address = cityDal.Address,
                country_code = cityDal.CountryCode,
                latitude = cityDal.Latitude,
                longitude = cityDal.Longitude
            });
            return rowsInserted == 1;
        }
        
        public async Task<bool> Update(City cityDal)
        {
            string sql =
                @"
UPDATE
    cities
SET
    name = @name,
    url_name = @url_name,
    address = @address,
    country_code = @country_code,
    latitude = @latitude,
    longitude = @longitude
WHERE
    id = @id";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsUpdated = await connection.ExecuteAsync(sql, new
            {
                id = cityDal.Id,
                name = cityDal.Name,
                url_name = cityDal.UrlName,
                address = cityDal.Address,
                country_code = cityDal.CountryCode,
                latitude = cityDal.Latitude,
                longitude = cityDal.Longitude
            });
            return rowsUpdated == 1;
        }
    }
}
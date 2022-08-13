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
    id = @Id";
            using var connection = await _connectionFactory.CreateConnection();
            var cityDal = await connection.QueryFirstOrDefaultAsync<City>(sql, new
            {
                Id = id
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
    LOWER(cities.name) LIKE CONCAT('%', @СityName, '%')";
            using var connection = await _connectionFactory.CreateConnection();
            var cityDal = await connection.QueryFirstOrDefaultAsync<City>(sql, new
            {
                СityName = cityName
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
    @Id,
    @Name,
    @UrlName,
    @Address,
    @CountryCode,
    @Latitude,
    @Longitude
)";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                cityDal.Id,
                cityDal.Name,
                cityDal.UrlName,
                cityDal.Address,
                cityDal.CountryCode,
                cityDal.Latitude,
                cityDal.Longitude
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
    name = @Name,
    url_name = @UrlName,
    address = @Address,
    country_code = @CountryCode,
    latitude = @Latitude,
    longitude = @Longitude
WHERE
    id = @Id";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsUpdated = await connection.ExecuteAsync(sql, new
            {
                cityDal.Id,
                cityDal.Name,
                cityDal.UrlName,
                cityDal.Address,
                cityDal.CountryCode,
                cityDal.Latitude,
                cityDal.Longitude
            });
            return rowsUpdated == 1;
        }
    }
}
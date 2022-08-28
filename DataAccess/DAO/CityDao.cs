using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.Dao.Interfaces;
using Domain.Entities;

namespace DataAccess.Dao
{
    public class CityDao : ICityDao
    {
        private readonly IConnectionFactory _connectionFactory;

        private const string SelectFieldNames = @"
    c.id              AS  Id,
    c.name            AS  Name,
    c.name_for_url    AS  NameForUrl,
    c.address         AS  Address,
    c.country_code    AS  CountryCode,
    c.latitude        AS  Latitude,
    c.longitude       AS  Longitude
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
    cities c
WHERE
    c.id = @Id";
            using var connection = await _connectionFactory.CreateConnection();
            var city = await connection.QueryFirstOrDefaultAsync<City>(sql, new
            {
                Id = id
            });
            return city;
        }
        
        public async Task<City?> GetCityByLowerCaseName(string cityName)
        {
            string sql =
                $@"
SELECT
    {SelectFieldNames}
FROM 
    cities c
WHERE
    LOWER(c.name) LIKE CONCAT('%', @СityName, '%')";
            using var connection = await _connectionFactory.CreateConnection();
            var city = await connection.QueryFirstOrDefaultAsync<City>(sql, new
            {
                СityName = cityName
            });
            return city;
        }
        
        public async Task<bool> Create(City city)
        {
            string sql =
                @"
INSERT INTO cities (
    id,
    name,
    name_for_url,
    address,
    country_code,
    latitude,
    longitude
)
VALUES (
    @Id,
    @Name,
    @NameForUrl,
    @Address,
    @CountryCode,
    @Latitude,
    @Longitude
)";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                city.Id,
                city.Name,
                city.NameForUrl,
                city.Address,
                city.CountryCode,
                city.Latitude,
                city.Longitude
            });
            return rowsInserted == 1;
        }
        
        public async Task<bool> Update(City city)
        {
            string sql =
                @"
UPDATE
    cities
SET
    name = @Name,
    name_for_url = @NameForUrl,
    address = @Address,
    country_code = @CountryCode,
    latitude = @Latitude,
    longitude = @Longitude
WHERE
    id = @Id";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsUpdated = await connection.ExecuteAsync(sql, new
            {
                city.Id,
                city.Name,
                city.NameForUrl,
                city.Address,
                city.CountryCode,
                city.Latitude,
                city.Longitude
            });
            return rowsUpdated == 1;
        }
    }
}
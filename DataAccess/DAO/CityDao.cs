using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.DAL;
using DataAccess.DAO.Interfaces;

namespace DataAccess.DAO
{
    public class CityDao : ICityDao
    {
        private readonly IConnectionFactory _connectionFactory;

        public CityDao(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<CityDal?> GetCityById(int id)
        {
            string sql =
                @"
SELECT
    id              AS  Id,
    name            AS  Name,
    url_name        AS  UrlName,
    address         AS  Address,
    country_code    AS  CountryCode,
    latitude        AS  Latitude,
    longitude       AS  Longitude
FROM 
    cities
WHERE
    id = @id;";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var cityDal = await connection.QueryFirstOrDefaultAsync<CityDal>(sql, new
            {
                id = id
            });
            return cityDal;
        }
        
        public async Task<bool> Create(CityDal cityDal)
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
);";
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
        
        public async Task<bool> Update(CityDal cityDal)
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
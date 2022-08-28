using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.Dao.Interfaces;
using Domain.Entities;

namespace DataAccess.Dao
{
    public class UserDao : IUserDao
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserDao(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByExternalId(long externalId)
        {
            string sql =
                @"
SELECT
    u.id            AS  Id,
    u.external_id   AS  ExternalId,
    u.city_id       AS  CityId,
    u.first_name    AS  FirstName,
    u.last_name     AS  LastName,
    u.user_name     AS  UserName,
    u.chat_id       AS  ChatId
FROM 
    users u
WHERE
    u.external_id = @ExternalId";
            using var connection = await _connectionFactory.CreateConnection();
            var dialogStateDal = await connection.QueryFirstOrDefaultAsync<User>(sql, new
            {
                ExternalId = externalId
            });
            return dialogStateDal;
        }
        
        public async Task<bool> Create(User user)
        {
            string sql =
                @"
INSERT INTO users (
    external_id,
    city_id,
    first_name,
    last_name,
    user_name,
    chat_id
)
VALUES (
    @ExternalId,
    @CityId,
    @FirstName,
    @LastName,
    @UserName,
    @ChatId
)";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                user.ExternalId,
                user.CityId,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.ChatId
            });
            return rowsInserted == 1;
        }

        public async Task<bool> UpdateCity(
            int userId, 
            int? cityId)
        {
            string sql =
                @"
UPDATE
    users
SET
    city_id = @CityId
WHERE
    id = @UserId";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsUpdated = await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                CityId = cityId
            });
            return rowsUpdated == 1;
        }
    }
}
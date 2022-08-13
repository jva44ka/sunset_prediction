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

        public async Task<User?> GetUserById(int userId)
        {
            string sql =
                @"
SELECT
    u.id                      AS  Id,
    u.city_id                 AS  CityId,
    u.first_name              AS  FirstName,
    u.last_name               AS  LastName,
    u.user_name               AS  UserName,
    u.previous_dialog_state   AS  PreviousDialogState,
    u.current_dialog_state    AS  CurrentDialogState,
    u.state_change_date       AS  StateChangeDate
FROM 
    users u
WHERE
    u.id = @UserId";
            using var connection = await _connectionFactory.CreateConnection();
            var dialogStateDal = await connection.QueryFirstOrDefaultAsync<User>(sql, new
            {
                UserId = userId
            });
            return dialogStateDal;
        }
        
        public async Task<bool> Create(User user)
        {
            string sql =
                @"
INSERT INTO users (
    id,
    city_id,
    first_name,
    last_name,
    user_name,
    previous_dialog_state,
    current_dialog_state,
    state_change_date
)
VALUES (
    @Id,
    @CityId,
    @FirstName,
    @LastName,
    @UserName,
    @PreviousDialogState,
    @CurrentDialogState,
    @StateChangeDate
)";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                user.Id,
                user.PreviousDialogState,
                user.CurrentDialogState,
                user.CityId,
                user.StateChangeDate,
                user.FirstName,
                user.LastName,
                user.UserName,
            });
            return rowsInserted == 1;
        }
        
        public async Task<bool> Update(User user)
        {
            string sql =
                @"
UPDATE
    users
SET
    previous_dialog_state = @PreviousDialogState,
    current_dialog_state = @CurrentDialogState,
    city_id = @CityId,
    state_change_date = @StateChangeDate
WHERE
    id = @Id";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsUpdated = await connection.ExecuteAsync(sql, new
            {
                user.Id,
                user.PreviousDialogState,
                user.CurrentDialogState,
                user.CityId,
                user.StateChangeDate
            });
            return rowsUpdated == 1;
        }
    }
}
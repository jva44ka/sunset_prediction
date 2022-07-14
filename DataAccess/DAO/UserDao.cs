using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.DAO.Interfaces;
using Domain.Entities;

namespace DataAccess.DAO
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
    id                          AS  Id,
    city_id                     AS  CityId,
    first_name                  AS  FirstName,
    last_name                   AS  LastName,
    user_name                   AS  UserName,
    previous_dialog_state       AS  PreviousDialogState,
    dialog_state                AS  DialogState,
    state_change_date           AS  StateChangeDate
FROM 
    users
WHERE
    id = @user_id";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var dialogStateDal = await connection.QueryFirstOrDefaultAsync<User>(sql, new
            {
                user_id = userId
            });
            return dialogStateDal;
        }
        
        public async Task<bool> Create(User user)
        {
            string sql =
                @"
INSERT INTO 
    users (
        id,
        previous_dialog_state,
        dialog_state,
        city_id,
        state_change_date,
        first_name,
        last_name,
        user_name
    )
VALUES (
    @id,
    @previous_dialog_state,
    @dialog_state,
    @city_id,
    @state_change_date,
    @first_name,
    @last_name,
    @user_name
)";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                id = user.Id,
                previous_dialog_state = user.PreviousDialogState,
                dialog_state = user.DialogState,
                city_id = user.CityId,
                state_change_date = user.StateChangeDate,
                first_name = user.FirstName,
                last_name = user.LastName,
                user_name = user.UserName,
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
    previous_dialog_state = @previous_dialog_state,
    dialog_state = @dialog_state,
    city_id = @city_id,
    state_change_date = @state_change_date
WHERE
    id = @id";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsUpdated = await connection.ExecuteAsync(sql, new
            {
                id = user.Id,
                previous_dialog_state = user.PreviousDialogState,
                dialog_state = user.DialogState,
                city_id = user.CityId,
                state_change_date = user.StateChangeDate
            });
            return rowsUpdated == 1;
        }
    }
}
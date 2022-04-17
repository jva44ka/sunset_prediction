using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.DAO.Interfaces;
using Domain.Entities;

namespace DataAccess.DAO
{
    public class UserDao : IDialogStateDao
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserDao(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<User?> GetStateByUserId(int userId)
        {
            string sql =
                @"
SELECT
    id                          AS  Id,
    previous_dialog_state       AS  PreviousDialogState,
    dialog_state                AS  DialogState,
    city_id                     AS  CityId,
    state_change_date           AS  StateChangeDate
FROM 
    users
WHERE
    id = @user_id;";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var dialogStateDal = await connection.QueryFirstOrDefaultAsync<User>(sql, new
            {
                user_id = userId
            });
            return dialogStateDal;
        }
        
        public async Task<bool> Create(User userDal)
        {
            string sql =
                @"
INSERT INTO 
    users (
        id,
        previous_dialog_state,
        dialog_state,
        city_id,
        state_change_date
    )
VALUES (
    @id,
    @previous_dialog_state,
    @dialog_state,
    @city_id,
    @state_change_date
);";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                id = userDal.Id,
                previous_dialog_state = userDal.PreviousDialogState,
                dialog_state = userDal.DialogState,
                city_id = userDal.CityId,
                state_change_date = userDal.StateChangeDate
            });
            return rowsInserted == 1;
        }
        
        public async Task<bool> Update(User userDal)
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
                id = userDal.Id,
                previous_dialog_state = userDal.PreviousDialogState,
                dialog_state = userDal.DialogState,
                city_id = userDal.CityId,
                state_change_date = userDal.StateChangeDate
            });
            return rowsUpdated == 1;
        }
    }
}
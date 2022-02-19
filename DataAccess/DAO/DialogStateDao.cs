using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.DAL;
using DataAccess.DAO.Interfaces;

namespace DataAccess.DAO
{
    public class DialogStateDao : IDialogStateDao
    {
        private readonly IConnectionFactory _connectionFactory;

        public DialogStateDao(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<DialogStateDal?> GetStateByUserId(int userId)
        {
            string sql =
                @"
SELECT
    user_id             AS  UserId,
    previous_state      AS  PreviousState,
    state               AS  State,
    proposed_city_id    AS  ProposedCityId,
    state_change_date   AS  StateChangeDate
FROM 
    dialog_states
WHERE
    user_id = @user_id;";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var dialogStateDal = await connection.QueryFirstOrDefaultAsync<DialogStateDal>(sql, new
            {
                user_id = userId
            });
            return dialogStateDal;
        }
        
        public async Task<bool> Create(DialogStateDal dialogStateDal)
        {
            string sql =
                @"
INSERT INTO 
    dialog_states (
    user_id,
    previous_state,
    state,
    proposed_city_id,
    state_change_date
)
VALUES (
    @user_id,
    @previous_state,
    @state,
    @proposed_city_id,
    @state_change_date
);";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                user_id = dialogStateDal.UserId,
                previous_state = dialogStateDal.PreviousState,
                state = dialogStateDal.State,
                proposed_city_id = dialogStateDal.ProposedCityId,
                state_change_date = dialogStateDal.StateChangeDate
            });
            return rowsInserted == 1;
        }
        
        public async Task<bool> Update(DialogStateDal dialogStateDal)
        {
            string sql =
                @"
UPDATE
    dialog_states
SET
    previous_state = @previous_state,
    state = @state,
    proposed_city_id = @proposed_city_id,
    state_change_date = @state_change_date
WHERE
    user_id = @user_id";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsUpdated = await connection.ExecuteAsync(sql, new
            {
                user_id = dialogStateDal.UserId,
                previous_state = dialogStateDal.PreviousState,
                state = dialogStateDal.State,
                proposed_city_id = dialogStateDal.ProposedCityId,
                state_change_date = dialogStateDal.StateChangeDate
            });
            return rowsUpdated == 1;
        }
    }
}
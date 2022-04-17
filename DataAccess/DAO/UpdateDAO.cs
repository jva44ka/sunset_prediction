using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.DAO.Interfaces;
using Domain.Entities;

namespace DataAccess.DAO
{
    public class UpdateDao : IUpdateDao
    {
        private readonly IConnectionFactory _connectionFactory;

        public UpdateDao(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<bool> Create(Update update)
        {
            string sql = 
@"
INSERT INTO 
    updates 
VALUES (
    @update_id,
    @handle_date
);";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                update_id = update.UpdateId,
                handle_date = update.HandleDate
            });
            return rowsInserted == 1;
        }

        public async Task<Update?> GetLastUpdate()
        {
            string sql =
                @"
SELECT
    update_id   AS  UpdateId
FROM
    updates
ORDER BY 
    update_id DESC
LIMIT 
    1";
            using var connection = await _connectionFactory.CreateConnection().ConfigureAwait(false);
            var update = await connection.QueryFirstOrDefaultAsync<Update>(sql).ConfigureAwait(false);
            return update;
        }
    }
}

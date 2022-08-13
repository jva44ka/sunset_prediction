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
    @UpdateId,
    @HandleDate
);";
            using var connection = await _connectionFactory.CreateConnection();
            var rowsInserted = await connection.ExecuteAsync(sql, new
            {
                update.UpdateId,
                update.HandleDate
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
            using var connection = await _connectionFactory.CreateConnection();
            var update = await connection.QueryFirstOrDefaultAsync<Update>(sql);
            return update;
        }
    }
}

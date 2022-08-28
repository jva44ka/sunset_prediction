using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.Dao.Interfaces;
using Domain.Entities;

namespace DataAccess.Dao;

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
INSERT INTO updates (
    external_id,
    handled_at
)
VALUES (
    @ExternalId,
    @HandledAt
);";
        using var connection = await _connectionFactory.CreateConnection();
        var rowsInserted = await connection.ExecuteAsync(sql, new
        {
            update.ExternalId,
            update.HandledAt
        });
        return rowsInserted == 1;
    }

    public async Task<Update?> GetLastUpdate()
    {
        string sql =
            @"
SELECT
    u.id            AS  Id,
    u.external_id   AS  ExternalId,
    u.handled_at    AS  HandledAt
FROM
    updates u
ORDER BY 
    u.id DESC
LIMIT 
    1";
        using var connection = await _connectionFactory.CreateConnection();
        var update = await connection.QueryFirstOrDefaultAsync<Update?>(sql);
        return update;
    }
}
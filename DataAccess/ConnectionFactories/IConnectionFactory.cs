using System.Data;
using System.Threading.Tasks;

namespace DataAccess.ConnectionFactories
{
    public interface IConnectionFactory
    {
        Task<IDbConnection> CreateConnection();
    }
}
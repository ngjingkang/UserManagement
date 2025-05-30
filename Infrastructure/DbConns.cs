using Domain.JsonConfigs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Infrastructure
{
    public sealed class DbConn(
        IOptionsMonitor<ConnectionStrings> connectionStrings)
    {
        public readonly ConnectionStrings _connectionStrings = connectionStrings.CurrentValue;

        public IDbConnection LocalDb => new SqlConnection(_connectionStrings.LocalDb);
    }
}

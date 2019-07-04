using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Karmr.Common.Contracts;

namespace Karmr.Persistence
{
    public sealed class QueryRepository : IQueryRepository
    {
        private readonly string connectionString;

        public QueryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public T QuerySingle<T>(string query)
        {
            return this.QuerySingle<T>(query, null);
        }

        public T QuerySingle<T>(string query, object @params)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                return connection.QuerySingle<T>(query, @params);
            }
        }

        public IEnumerable<T> Query<T>(string query)
        {
            return this.Query<T>(query, null);
        }

        public IEnumerable<T> Query<T>(string query, object @params)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                return connection.Query<T>(query, @params);
            }
        }
    }
}
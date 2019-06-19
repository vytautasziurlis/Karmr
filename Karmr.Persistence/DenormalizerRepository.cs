using System.Data.SqlClient;
using Dapper;
using Karmr.Common.Contracts;

namespace Karmr.Persistence
{
    public class DenormalizerRepository : IDenormalizerRepository
    {
        private readonly string connectionString;

        public DenormalizerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Execute(string query, object @params)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Execute(query, @params);
            }
        }
    }
}
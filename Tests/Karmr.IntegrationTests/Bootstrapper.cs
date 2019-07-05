namespace Karmr.IntegrationTests
{
    using Dapper;
    using NUnit.Framework;
    using System.Configuration;
    using System.Data.SqlClient;

    [SetUpFixture]
    public class Bootstrapper
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                //connection.Execute("TRUNCATE TABLE [Events]");
            }
        }
    }
}
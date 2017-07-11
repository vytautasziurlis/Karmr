namespace Karmr.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using Karmr.Contracts;

    using Newtonsoft.Json;
    using Dapper;

    internal class SqlEventRepository : IEventRepository
    {
        private const string InsertSql = @"INSERT INTO [dbo].[Events] ([EntityType], [EntityKey], [Sequence], [EventType], [EventPayload])
                                            VALUES (@EntityType, @EntityKey, @Sequence, @EventType, @EventPayload)";

        private const string SelectSql = @"SELECT [Sequence], [EventType], [EventPayload] FROM [dbo].[Events] WHERE EntityKey = @EntityKey ORDER BY [Sequence]";

        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new JsonContractResolver() };

        private readonly string connectionString;

        public SqlEventRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Save(Type entityType, Guid entityKey, IEvent @event, int sequenceNumber)
        {
            var eventTypeName = @event.GetType().AssemblyQualifiedName;
            var eventPayload = JsonConvert.SerializeObject(@event, this.jsonSettings);

            using (var connection = new SqlConnection(this.connectionString))
            {
                var @params = new
                                  {
                                      entityType = entityType.AssemblyQualifiedName,
                                      entityKey,
                                      sequence = sequenceNumber,
                                      eventType = eventTypeName,
                                      eventPayload
                                  };
                connection.Execute(InsertSql, @params);
            }
        }

        public IEnumerable<IEvent> Get(Guid entityKey)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                return conn.Query<dynamic>(SelectSql, new { entityKey })
                    .Select(x => this.DeserializeEvent(x.EventType, x.EventPayload))
                    .Cast<IEvent>();
            }
        }

        private IEvent DeserializeEvent(string eventTypeName, string eventPayload)
        {
            return (IEvent)JsonConvert.DeserializeObject(eventPayload, Type.GetType(eventTypeName), this.jsonSettings);
        }
    }
}
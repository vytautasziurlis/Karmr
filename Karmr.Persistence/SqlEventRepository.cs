namespace Karmr.Persistence
{
    using System;
    using System.Collections.Generic;

    using Karmr.Contracts;

    using Newtonsoft.Json;

    internal class SqlEventRepository : IEventRepository
    {
        private const string InsertSql = @"";

        public void Save(IEvent @event, Type entityType, int sequenceNumber)
        {
            var eventTypeName = @event.GetType().AssemblyQualifiedName;
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new JsonContractResolver() };
            var serializedEvent = JsonConvert.SerializeObject(@event, jsonSettings);



            throw new NotImplementedException();
        }

        public IEnumerable<IEvent> Get(Type entityType, Guid entityKey)
        {
            throw new NotImplementedException();
        }
    }
}
namespace Karmr.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IEventRepository
    {
        void Save(IEvent @event, Type entityType, int sequenceNumber);

        IEnumerable<IEvent> Get(Type entityType, Guid entityKey);
    }
}
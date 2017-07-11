namespace Karmr.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IEventRepository
    {
        void Save(Type entityType, Guid entityKey, IEvent @event, int sequenceNumber);

        IEnumerable<IEvent> Get(Guid entityKey);
    }
}
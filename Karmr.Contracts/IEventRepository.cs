namespace Karmr.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IEventRepository
    {
        void Save(IEvent @event, int sequenceNumber);

        IEnumerable<IEvent> Get(Type entityType, Guid entityKey);
    }
}
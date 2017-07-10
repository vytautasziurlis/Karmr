namespace Karmr.Domain.Events
{
    using System;

    using Karmr.Contracts;

    internal abstract class Event : IEvent
    {
        internal Guid EntityKey { get; }

        internal Guid UserId { get; }

        internal DateTime Timestamp { get; }

        internal Event(Guid entityKey, Guid userId)
        {
            this.EntityKey = entityKey;
            this.UserId = userId;
            this.Timestamp = DateTime.UtcNow;
        }
    }
}
using System;

namespace Karmr.Domain.Events
{
    internal class ListingDiscussionThreadCreated : Event
    {
        internal Guid ThreadId { get; }

        public ListingDiscussionThreadCreated(Guid entityKey, Guid userId, Guid threadId, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.ThreadId = threadId;
        }
    }
}
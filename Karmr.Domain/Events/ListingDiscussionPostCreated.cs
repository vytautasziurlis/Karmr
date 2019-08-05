using System;

namespace Karmr.Domain.Events
{
    internal class ListingDiscussionPostCreated : Event
    {
        internal Guid PostId { get; }

        internal Guid ThreadId { get; }

        internal string Content { get; }

        public ListingDiscussionPostCreated(Guid entityKey, Guid userId, Guid postId, Guid threadId, string content, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.PostId = postId;
            this.ThreadId = threadId;
            this.Content = content;
        }
    }
}
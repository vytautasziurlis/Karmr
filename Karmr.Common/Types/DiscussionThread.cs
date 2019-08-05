using System;
using System.Collections.Generic;

namespace Karmr.Common.Types
{
    public sealed class DiscussionThread
    {
        public Guid ThreadId { get; }

        public Guid UserId { get; }

        public List<DiscussionPost> Posts { get; }

        public DiscussionThread(Guid threadId, Guid userId)
        {
            this.ThreadId = threadId;
            this.UserId = userId;
            this.Posts = new List<DiscussionPost>();
        }
    }
}
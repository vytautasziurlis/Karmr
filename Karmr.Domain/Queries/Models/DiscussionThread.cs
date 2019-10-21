using System;
using System.Collections.Generic;

namespace Karmr.Domain.Queries.Models
{
    public sealed class DiscussionThread
    {
        public Guid ThreadId { get; }

        public Guid UserId { get; }

        public IEnumerable<DiscussionPost> Posts { get; }

        public DiscussionThread(Guid threadId, Guid userId, IEnumerable<DiscussionPost> posts)
        {
            ThreadId = threadId;
            UserId = userId;
            Posts = posts;
        }
    }
}
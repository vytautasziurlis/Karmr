using System;

namespace Karmr.Domain.Queries.Models
{
    public sealed class DiscussionPost
    {
        public Guid UserId { get; }

        public string Content { get; }

        public DateTime Created { get; }

        public DiscussionPost(Guid userId, string content, DateTime created)
        {
            UserId = userId;
            Content = content;
            Created = created;
        }
    }
}
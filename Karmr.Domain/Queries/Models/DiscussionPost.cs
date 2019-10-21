using System;

namespace Karmr.Domain.Queries.Models
{
    public sealed class DiscussionPost
    {
        public Guid UserId { get; }

        public string Content { get; }

        public DiscussionPost(Guid userId, string content)
        {
            UserId = userId;
            Content = content;
        }
    }
}
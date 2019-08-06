using System;

namespace Karmr.Common.Types
{
    public sealed class DiscussionPost
    {
        public Guid UserId { get; }

        public string Content { get; }

        public DiscussionPost(Guid userId, string content)
        {
            this.UserId = userId;
            this.Content = content;
        }
    }
}
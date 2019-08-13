using System;

namespace Karmr.Domain.Commands
{
    public sealed class CreateListingDiscussionPostCommand : Command
    {
        public Guid ThreadId { get; }

        public string Content { get; }

        public CreateListingDiscussionPostCommand(Guid entityKey, Guid userId, Guid threadId, string content) : base(entityKey, userId)
        {
            this.ThreadId = threadId;
            this.Content = content;
        }
    }
}
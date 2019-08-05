using System;

namespace Karmr.Domain.Commands
{
    public sealed class CreateListingDiscussionThreadCommand : Command
    {
        public string Content { get; }

        public CreateListingDiscussionThreadCommand(Guid entityKey, Guid userId, string content) : base(entityKey, userId)
        {
            this.Content = content;
        }
    }
}
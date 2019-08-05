using System;

namespace Karmr.Domain.Commands
{
    public sealed class ArchiveListingCommand : Command
    {
        public ArchiveListingCommand(Guid entityKey, Guid userId) : base(entityKey, userId)
        {
        }
    }
}
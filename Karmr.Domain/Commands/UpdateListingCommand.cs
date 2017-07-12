namespace Karmr.Domain.Commands
{
    using System;

    public class UpdateListingCommand : Command
    {
        public string Description { get; }

        public UpdateListingCommand(Guid entityKey, Guid userId, string description) : base(entityKey, userId)
        {
            this.Description = description;
        }
    }
}
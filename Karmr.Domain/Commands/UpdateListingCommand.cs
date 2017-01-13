namespace Karmr.Domain.Commands
{
    using System;

    public class UpdateListingCommand : Command
    {
        public string Description { get; private set; }

        public UpdateListingCommand(Guid userId, string description) : base(userId)
        {
            this.Description = description;
        }
    }
}
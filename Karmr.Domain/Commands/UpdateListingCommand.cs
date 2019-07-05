namespace Karmr.Domain.Commands
{
    using System;

    public class UpdateListingCommand : Command
    {
        public string Name { get; }

        public string Description { get; }

        public UpdateListingCommand(Guid entityKey, Guid userId, string name, string description) : base(entityKey, userId)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
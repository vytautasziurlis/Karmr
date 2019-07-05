namespace Karmr.Domain.Commands
{
    using System;

    public class CreateListingCommand : Command
    {
        public string Name { get; }

        public string Description { get; }

        public CreateListingCommand(Guid userId, string name, string description) : base(userId)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
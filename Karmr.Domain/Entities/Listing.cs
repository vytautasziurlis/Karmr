using Karmr.Domain.Commands;

namespace Karmr.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Contracts.Commands;

    internal class Listing : Entity
    {
        internal Guid Id { get; private set; }

        internal Guid UserId { get; private set; }

        internal string Description { get; private set; }

        internal Listing(IEnumerable<ICommand> commands) : base(commands) { }

        private void Handle(CreateListingCommand command)
        {
            if (this.commands.Any())
            {
                throw new Exception(string.Format("Expected empty list of commands, found {0} commands", this.commands.Count));
            }

            this.Id = command.EntityKey;
            this.UserId = command.UserId;
            this.Description = command.Description;
        }

        private void Handle(UpdateListingCommand command)
        {
            if (!this.commands.Any(x => x is CreateListingCommand))
            {
                throw new Exception(string.Format("CreateListingCommand commands missing (found {0} commands)", this.commands.Count));
            }
            if (this.UserId != command.UserId)
            {
                throw new Exception("Permission denied");
            }

            this.Description = command.Description;
        }
    }
}
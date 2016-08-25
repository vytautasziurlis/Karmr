using Karmr.Domain.Commands;

namespace Karmr.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Contracts.Commands;

    internal class Listing : Aggregate
    {
        internal string Description { get; private set; }

        internal Listing(IEnumerable<ICommand> commands) : base(commands) { }

        private void Handle(CreateListingCommand command)
        {
            if (this.commands.Any())
            {
                throw new Exception(string.Format("Expected empty list of commands, found {0} commands", this.commands.Count));
            }

            this.Description = command.Description;
        }

        private void Handle(UpdateListingCommand command)
        {
            this.Description = command.Description;
        }
    }
}
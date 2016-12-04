namespace Karmr.Domain.Commands
{
    using System;

    public class CreateListingCommand : Command
    {
        public string Description { get; set; }

        public CreateListingCommand() : base()
        {
        }
    }
}
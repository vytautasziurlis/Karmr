using Karmr.Domain.Commands;

namespace Karmr.Domain.Entities
{
    public class Listing : Aggregate
    {
        public string Description { get; private set; }

        public Listing() : base()
        {
        }

        internal bool Handle(CreateListingCommand command)
        {
            this.Description = command.Description;
            return true;
        }

        internal bool Handle(UpdateListingCommand command)
        {
            this.Description = command.Description;
            return true;
        }
    }
}

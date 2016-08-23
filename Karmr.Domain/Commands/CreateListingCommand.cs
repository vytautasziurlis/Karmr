namespace Karmr.Domain.Commands
{
    public class CreateListingCommand : Command
    {
        public string Description { get; private set; }

        public CreateListingCommand()
        {

        }

        public CreateListingCommand(string description)
        {
            this.Description = description;
        }
    }
}
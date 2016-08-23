namespace Karmr.Domain.Commands
{
    public class UpdateListingCommand : Command
    {
        public string Description { get; private set; }

        public UpdateListingCommand()
        {

        }

        public UpdateListingCommand(string description)
        {
            this.Description = description;
        }
    }
}
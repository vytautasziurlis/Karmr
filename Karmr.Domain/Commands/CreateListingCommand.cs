using System;
using Karmr.Common.Types;

namespace Karmr.Domain.Commands
{
    public class CreateListingCommand : Command
    {
        public string Name { get; }

        public string Description { get; }

        public string LocationName { get; }

        public GeoLocation? Location { get; }

        public CreateListingCommand(Guid userId, string name, string description, string locationName, GeoLocation? location) : base(userId)
        {
            this.Name = name;
            this.Description = description;
            this.LocationName = locationName;
            this.Location = location;
        }
    }
}
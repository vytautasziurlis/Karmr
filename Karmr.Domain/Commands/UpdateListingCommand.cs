using System;
using Karmr.Common.Types;

namespace Karmr.Domain.Commands
{
    public class UpdateListingCommand : Command
    {
        public string Name { get; }

        public string Description { get; }

        public string LocationName { get; }

        public GeoLocation? Location { get; }

        public UpdateListingCommand(Guid entityKey, Guid userId, string name, string description, string locationName, GeoLocation? location) : base(entityKey, userId)
        {
            this.Name = name;
            this.Description = description;
            this.LocationName = locationName;
            this.Location = location;
        }
    }
}
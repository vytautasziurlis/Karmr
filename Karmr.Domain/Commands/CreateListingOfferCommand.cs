using System;

namespace Karmr.Domain.Commands
{
    public sealed class CreateListingOfferCommand : Command
    {
        public CreateListingOfferCommand(Guid entityKey, Guid userId) : base(entityKey, userId)
        {
        }
    }
}
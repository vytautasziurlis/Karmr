using System;

namespace Karmr.Domain.Commands
{
    public sealed class AcceptListingOfferCommand : Command
    {
        public Guid OfferId { get; }

        public AcceptListingOfferCommand(Guid entityKey, Guid userId, Guid offerId) : base(entityKey, userId)
        {
            this.OfferId = offerId;
        }
    }
}
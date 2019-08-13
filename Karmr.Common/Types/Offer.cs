using System;

namespace Karmr.Common.Types
{
    public sealed class Offer
    {
        public Guid OfferId { get; }

        public Guid UserId { get; }

        public bool Accepted { get; }

        public Offer(Guid offerId, Guid userId, bool accepted)
        {
            this.OfferId = offerId;
            this.UserId = userId;
            this.Accepted = accepted;
        }
    }
}
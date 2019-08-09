using System;

namespace Karmr.Common.Types
{
    public sealed class Offer
    {
        public Guid UserId { get; }

        public bool Accepted { get; }

        public Offer(Guid userId, bool accepted)
        {
            this.UserId = userId;
            this.Accepted = accepted;
        }
    }
}
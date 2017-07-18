namespace Karmr.DomainUnitTests.Helpers
{
    using System;

    using Karmr.Common.Contracts;
    internal class StaticClock : IClock
    {
        private readonly DateTime now;

        internal StaticClock(DateTime now)
        {
            this.now = now;
        }

        public DateTime UtcNow
        {
            get
            {
                return this.now;
            }
        }
    }
}
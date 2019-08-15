using System;
using Karmr.Common.Contracts;

namespace Karmr.Common.Infrastructure
{
    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
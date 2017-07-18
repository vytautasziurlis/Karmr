namespace Karmr.Common.Contracts
{
    using System;

    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
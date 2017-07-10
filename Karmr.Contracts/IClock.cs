namespace Karmr.Contracts
{
    using System;

    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
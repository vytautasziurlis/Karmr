namespace Karmr.Domain.Infrastructure
{
    using System;

    public class UnhandledCommandException : Exception
    {
        public UnhandledCommandException(string message) : base(message)
        {
        }
    }
}
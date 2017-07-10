namespace Karmr.Domain.Infrastructure
{
    using System;

    public class UnhandledEventException : Exception
    {
        public UnhandledEventException(string message) : base(message)
        {
        }
    }
}
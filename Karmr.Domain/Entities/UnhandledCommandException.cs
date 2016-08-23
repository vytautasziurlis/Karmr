using System;

namespace Karmr.Domain.Entities
{
    public class UnhandledCommandException : Exception
    {
        public UnhandledCommandException(string message) : base(message)
        {
        }
    }
}
using System;

namespace Karmr.Contracts
{
    public interface ICommand
    {
        Guid EntityKey { get; }

        void Validate();
    }
}
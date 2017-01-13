using System;

namespace Karmr.Contracts.Commands
{
    public interface ICommand
    {
        Guid EntityKey { get; }

        void Validate();
    }
}
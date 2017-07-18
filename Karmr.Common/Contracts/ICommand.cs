namespace Karmr.Common.Contracts
{
    using System;

    public interface ICommand
    {
        Guid EntityKey { get; }

        void Validate();
    }
}
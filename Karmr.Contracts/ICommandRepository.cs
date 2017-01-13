namespace Karmr.Contracts
{
    using System;
    using System.Collections.Generic;

    using Karmr.Contracts.Commands;

    public interface ICommandRepository
    {
        void Save(ICommand command);

        IEnumerable<ICommand> Get(Type entityType, Guid entityKey);
    }
}
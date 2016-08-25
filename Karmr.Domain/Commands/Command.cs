using System;
using Karmr.Contracts.Commands;

namespace Karmr.Domain.Commands
{
    public abstract class Command : ICommand
    {
        public Guid EntityKey { get; protected set; }

        public long Sequence { get; }
        
        public Command()
        {
            this.EntityKey = Guid.NewGuid();
        }

        public override string ToString()
        {
            return string.Format("{0}: ", nameof(this.GetType));
        }
    }
}
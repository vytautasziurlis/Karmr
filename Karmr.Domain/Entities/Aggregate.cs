using Karmr.Contracts.Commands;
using Karmr.Domain.Commands;
using Karmr.Domain.Helpers;

using System.Collections.Generic;
using System.Reflection;

namespace Karmr.Domain.Entities
{
    using System;

    using Karmr.Domain.Infrastructure;

    internal abstract class Aggregate
    {
        private const BindingFlags BindingAttr = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        protected readonly IList<Command> commands  = new List<Command>();

        internal IEnumerable<ICommand> GetCommands()
        {
            return this.commands;
        }

        protected Aggregate(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                this.Handle(command);
            }
        }

        internal void Handle(ICommand command)
        {
            this.HandleImpl(command);
            this.commands.Add(command as Command);
        }

        private void HandleImpl(ICommand command)
        {
            var handleMethod = this.GetType().GetMethodBySignature(typeof(void),
                new[] { command.GetType() },
                BindingAttr);

            if (handleMethod == null)
            {
                throw new UnhandledCommandException(
                    string.Format("Handle for command {0} not found on entity {1}",
                    command.GetType(), this.GetType()));
            }

            // invoke wrapps any exception in TargetInvocationException,
            // to simplify debugging let's throw inner exception instead
            try
            {
                handleMethod.Invoke(this, new[] { command });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }
    }
}
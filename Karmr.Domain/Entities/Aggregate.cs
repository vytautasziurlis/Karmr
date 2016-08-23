using Karmr.Contracts.Commands;
using Karmr.Domain.Commands;
using Karmr.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Karmr.Domain.Entities
{
    public abstract class Aggregate
    {
        private const BindingFlags BindingAttr = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        private IList<Command> Commands
        {
            get;
            set;
        }

        public IEnumerable<ICommand> GetCommands()
        {
            return this.Commands;
        }

        public Aggregate()
        {
            this.Commands = new List<Command>();
        }

        public bool Handle(ICommand command)
        {
            if (this.HandleImpl(command))
            {
                this.Commands.Add(command as Command);
                return true;
            }
            return false;
        }

        private bool HandleImpl(ICommand command)
        {
            var handleMethod = this.GetType().GetMethodBySignature(typeof(bool),
                new[] { command.GetType() },
                BindingAttr);

            if (handleMethod == null)
            {
                throw new UnhandledCommandException(
                    string.Format("Handle for command {0} not found on entity {1}",
                    command.GetType(), this.GetType()));
            }
            var result = handleMethod.Invoke(this, new[] { command });
            return Convert.ToBoolean(result);
        }
    }
}
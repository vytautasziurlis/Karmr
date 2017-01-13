using System;
using Karmr.Domain.Commands;
using Karmr.Domain.Entities;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Karmr.Contracts.Commands;

namespace Karmr.DomainUnitTests.Entities
{
    public class EntityTests
    {
        [Test]
        public void NewEntityHasEmptyCommandList()
        {
            var subject = this.GetSubject(null);
            Assert.IsEmpty(subject.GetCommands());
        }

        [Test]
        public void HandlingCommandUpdatesCommands()
        {
            var subject = this.GetSubject(x => { });
            var command1 = new ConcreteCommand();
            var commands = this.HandleCommand(subject, command1).ToList();
            Assert.AreEqual(1, commands.Count);
            Assert.AreSame(command1, commands.First());

            var command2 = new ConcreteCommand();
            commands = this.HandleCommand(subject, command2).ToList();
            Assert.AreEqual(2, commands.Count);
            Assert.AreSame(command1, commands.First());
            Assert.AreSame(command2, commands.Last());
        }

        [Test]
        public void HandlingFalsyCommandDoesNotChangeState()
        {
            var subject = this.GetSubject(x => { });
            subject.Handle(new ConcreteCommand());
            Assert.IsEmpty(subject.GetCommands());
        }

        [Test]
        public void HandlingExceptionyCommandThrowsException()
        {
            var subject = this.GetSubject(x => { throw new Exception(); });
            Assert.Throws<Exception>(() => subject.Handle(new ConcreteCommand()));
        }

        private IEnumerable<ICommand> HandleCommand(Entity entity, Command command)
        {
            entity.Handle(command);
            return entity.GetCommands();
        }

        private ConcreteEntity GetSubject(Action<Command> func)
        {
            return new ConcreteEntity(func);
        }

        private class ConcreteEntity : Entity
        {
            private Action<Command> HandleFunc { get; }

            public ConcreteEntity(Action<Command> func) : base(new List<ICommand>())
            {
                this.HandleFunc = func;
            }

            internal void Handle(ConcreteCommand command)
            {
                this.HandleFunc.Invoke(command);
            }
        }

        private class ConcreteCommand : Command
        {
            internal ConcreteCommand() : base(Guid.Empty)
            {
            }
        }
    }
}
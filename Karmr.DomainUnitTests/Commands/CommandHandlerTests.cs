using NUnit.Framework;
using Karmr.Domain.Commands;
using Karmr.Contracts;
using Moq;
using Karmr.Domain.Infrastructure;
using Karmr.Contracts.Commands;

namespace Karmr.DomainUnitTests.Commands
{
    using System;
    using System.Collections.Generic;

    using Karmr.Domain.Entities;

    public class CommandHandlerTests
    {
        private Mock<ICommandRepository> mockRepo;

        [SetUp]
        public void Setup()
        {
            this.mockRepo = new Mock<ICommandRepository>();
        }

        [Test]
        public void CommandHandlerFailsWhenHandlerAintThere()
        {
            var subject = this.GetSubject(new List<Type>());
            Assert.Throws<UnhandledCommandException>(() => subject.Handle(Mock.Of<ICommand>()));
        }

        [Test]
        public void CommandHandlerFailsWhenCommandIsHandlerByMultipleAggregates()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Aggregate1), typeof(Aggregate2) });
            Assert.Throws<UnhandledCommandException>(() => subject.Handle(new DummyCommand1()));
        }

        [Test]
        public void CommandHandlerTriesToLoadAggregateCommands()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Aggregate1) });

            var command = new DummyCommand1();
            subject.Handle(command);

            this.mockRepo.Verify(x => x.Get(typeof(Aggregate1), command.EntityKey), Times.Once);
        }

        [Test]
        public void CommandHandlerPersistsCommandToRepository()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Aggregate1) });

            var command = new DummyCommand1();
            subject.Handle(command);

            this.mockRepo.Verify(x => x.Save(command), Times.Once);
        }

        private CommandHandler GetSubject(IEnumerable<Type> aggregateTypes)
        {
            return new CommandHandler(this.mockRepo.Object, aggregateTypes);
        }

        private class DummyCommand1 : Command { }

        private class Aggregate1 : Aggregate
        {
            private Aggregate1(IEnumerable<ICommand> commands) : base(commands) { }

            private void Handle(DummyCommand1 command) { }
        }

        private class Aggregate2 : Aggregate
        {
            private Aggregate2(IEnumerable<ICommand> commands) : base(commands) { }

            private void Handle(DummyCommand1 command) { }
        }
    }
}
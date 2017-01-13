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
        public void CommandHandlerFailsWhenCommandIsHandlerByMultipleEntities()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Entity1), typeof(Entity2) });
            Assert.Throws<UnhandledCommandException>(() => subject.Handle(new DummyCommand1()));
        }

        [Test]
        public void CommandHandlerTriesToLoadEntityCommands()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Entity1) });

            var command = new DummyCommand1();
            subject.Handle(command);

            this.mockRepo.Verify(x => x.Get(typeof(Entity1), command.EntityKey), Times.Once);
        }

        [Test]
        public void CommandHandlerPersistsCommandToRepository()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Entity1) });

            var command = new DummyCommand1();
            subject.Handle(command);

            this.mockRepo.Verify(x => x.Save(command), Times.Once);
        }

        private CommandHandler GetSubject(IEnumerable<Type> entityTypes)
        {
            return new CommandHandler(this.mockRepo.Object, entityTypes);
        }

        private class DummyCommand1 : Command
        {
            internal DummyCommand1() : base(Guid.Empty)
            {
            }
        }

        private class Entity1 : Entity
        {
            private Entity1(IEnumerable<ICommand> commands) : base(commands) { }

            private void Handle(DummyCommand1 command) { }
        }

        private class Entity2 : Entity
        {
            private Entity2(IEnumerable<ICommand> commands) : base(commands) { }

            private void Handle(DummyCommand1 command) { }
        }
    }
}
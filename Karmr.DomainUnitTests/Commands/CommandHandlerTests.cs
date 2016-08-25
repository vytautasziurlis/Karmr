using NUnit.Framework;
using Karmr.Domain.Commands;
using Karmr.Contracts;
using Moq;
using Karmr.Domain.Infrastructure;
using Karmr.Contracts.Commands;

namespace Karmr.DomainUnitTests.Commands
{
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
            var subject = this.GetSubject();
            Assert.Throws<UnhandledCommandException>(() => subject.Handle(Mock.Of<ICommand>()));
        }

        [Test]
        public void CommandHandlerCallsRepository()
        {
            var subject = this.GetSubject();

            //subject.Handle()
        }

        private CommandHandler GetSubject()
        {
            return new CommandHandler(this.mockRepo.Object);
        }
    }
}
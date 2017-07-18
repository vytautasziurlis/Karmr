using NUnit.Framework;
using Karmr.Domain.Commands;
using Karmr.DomainUnitTests.Builders;
using Karmr.Common.Infrastructure;

namespace Karmr.DomainUnitTests.Commands
{
    public class CreateListingCommandTests
    {
        private CommandBuilder<CreateListingCommand> commandBuilder;

        [SetUp]
        public void SetUp()
        {
            this.commandBuilder = new CommandBuilder<CreateListingCommand>();
        }

        [Test]
        public void ValidationDoesNotThrowForValidCommand()
        {
            var subject = this.commandBuilder.Build();

            Assert.DoesNotThrow(() => subject.Validate());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void DescriptionIsRequired(string value)
        {
            var subject = this.commandBuilder.With(x => x.Description, value).Build();

            Assert.Throws<CommandValidationException>(() => subject.Validate());
        }
    }
}
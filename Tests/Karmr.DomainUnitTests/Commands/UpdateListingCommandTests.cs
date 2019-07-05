using NUnit.Framework;
using Karmr.Domain.Commands;
using Karmr.DomainUnitTests.Builders;
using Karmr.Common.Infrastructure;

namespace Karmr.DomainUnitTests.Commands
{
    public class UpdateListingCommandTests
    {
        private CommandBuilder<UpdateListingCommand> commandBuilder;

        [SetUp]
        public void SetUp()
        {
            this.commandBuilder = new CommandBuilder<UpdateListingCommand>();
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
        [TestCase("\t")]
        public void NameIsRequired(string value)
        {
            var subject = this.commandBuilder.With(x => x.Name, value).Build();

            Assert.Throws<CommandValidationException>(() => subject.Validate());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\t")]
        public void DescriptionIsRequired(string value)
        {
            var subject = this.commandBuilder.With(x => x.Description, value).Build();

            Assert.Throws<CommandValidationException>(() => subject.Validate());
        }
    }
}
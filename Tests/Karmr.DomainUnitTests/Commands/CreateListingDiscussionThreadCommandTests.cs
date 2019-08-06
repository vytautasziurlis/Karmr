using NUnit.Framework;
using Karmr.Domain.Commands;
using Karmr.DomainUnitTests.Builders;
using Karmr.Common.Infrastructure;

namespace Karmr.DomainUnitTests.Commands
{
    public class CreateListingDiscussionThreadCommandTests
    {
        private CommandBuilder<CreateListingDiscussionThreadCommand> commandBuilder;

        [SetUp]
        public void SetUp()
        {
            this.commandBuilder = new CommandBuilder<CreateListingDiscussionThreadCommand>();
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
        public void DescriptionIsRequired(string value)
        {
            var subject = this.commandBuilder.With(x => x.Content, value).Build();

            Assert.Throws<CommandValidationException>(() => subject.Validate());
        }
    }
}
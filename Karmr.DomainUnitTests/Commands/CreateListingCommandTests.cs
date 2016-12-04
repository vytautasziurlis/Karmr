using NUnit.Framework;
using Karmr.Domain.Commands;
using Karmr.DomainUnitTests.Builders;
using Karmr.Domain.Infrastructure;

namespace Karmr.DomainUnitTests.Commands
{
    public class CreateListingCommandTests
    {
        [Test]
        public void ValidationDoesNotThrowForValidCommand()
        {
            var subject = this.GetSubject().Build();

            Assert.DoesNotThrow(() => subject.Validate());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void DescriptionIsRequired(string value)
        {
            var subject = this.GetSubject().With(x => x.Description = value).Build();

            Assert.Throws<CommandValidationException>(() => subject.Validate());
        }

        private CommandBuilder<CreateListingCommand> GetSubject()
        {
            return new CommandBuilder<CreateListingCommand>();
        }
    }
}
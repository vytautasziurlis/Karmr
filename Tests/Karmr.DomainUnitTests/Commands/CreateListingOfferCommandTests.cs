using NUnit.Framework;
using Karmr.Domain.Commands;
using Karmr.DomainUnitTests.Builders;

namespace Karmr.DomainUnitTests.Commands
{
    public class CreateListingOfferCommandTests
    {
        private CommandBuilder<CreateListingOfferCommand> commandBuilder;

        [SetUp]
        public void SetUp()
        {
            this.commandBuilder = new CommandBuilder<CreateListingOfferCommand>();
        }

        [Test]
        public void ValidationDoesNotThrowForValidCommand()
        {
            var subject = this.commandBuilder.Build();

            Assert.DoesNotThrow(() => subject.Validate());
        }
    }
}
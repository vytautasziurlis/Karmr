using Karmr.Domain.Entities;
using NUnit.Framework;
using System.Linq;
using Karmr.Domain.Commands;

namespace Karmr.DomainUnitTests.Entities
{
    using System;
    using System.Collections.Generic;

    using Karmr.Contracts.Commands;
    using Builders;

    public class ListingTests
    {
        [Test]
        public void HandlingCreateListingCommandTwiceThrowsException()
        {
            var subject = this.GetSubject();

            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void ListingHandlesCreateCommand()
        {
            var listing = this.GetSubject();
            var command = new CommandBuilder<CreateListingCommand>().Build();
            listing.Handle(command);

            Assert.AreEqual(command.Description, listing.Description);
            Assert.AreSame(command, listing.GetCommands().Single());
        }

        private Listing GetSubject()
        {
            return new Listing(new List<ICommand>());
        }
    }
}
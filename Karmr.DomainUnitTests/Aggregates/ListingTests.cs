using Karmr.Domain.Entities;
using NUnit.Framework;
using System.Linq;
using Karmr.Domain.Commands;

namespace Karmr.DomainUnitTests.Aggregates
{
    using System.Collections.Generic;

    using Karmr.Contracts.Commands;

    using Moq;

    public class ListingTests
    {
        [Test]
        public void ListingHandlesCreateCommand()
        {
            var listing = GetSubject();
            var command = new CreateListingCommand("d");
            listing.Handle(command);

            //var cmd = Builder.Command<CreateListingCommand>().With(x => new object());

            Assert.AreEqual(command.Description, listing.Description);
            Assert.AreSame(command, listing.GetCommands().Single());
        }

        private static Listing GetSubject()
        {
            return new Listing(new List<ICommand>());
        }
    }
}
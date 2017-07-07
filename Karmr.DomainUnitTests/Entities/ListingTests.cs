using Karmr.Domain.Entities;
using NUnit.Framework;
using Karmr.Domain.Commands;

namespace Karmr.DomainUnitTests.Entities
{
    using System;
    using System.Collections.Generic;

    using Builders;

    using Karmr.Domain.Events;

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
            var subject = this.GetSubject();
            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            Assert.AreEqual(command.EntityKey, subject.Id);
            Assert.AreEqual(command.UserId, subject.UserId);
            Assert.AreEqual(command.Description, subject.Description);
            Assert.AreSame(command, subject.GetCommands().Single());
        }

        [Test]
        public void HandlingUpdateListingCommandRequiresCreateCommand()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<UpdateListingCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingUpdateListingCommandChecksUserId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>().With(x => x.UserId, Guid.NewGuid()).Build();

            Assert.Throws<Exception>(() => subject.Handle(updateCommand));
        }

        [Test]
        public void ListingHandlesUpdateCommand()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .With(x => x.Description, createCommand.Description + " tail")
                .Build();

            subject.Handle(updateCommand);

            Assert.AreEqual(updateCommand.Description, subject.Description);
        }

        private Listing GetSubject()
        {
            return new Listing(new List<Event>());
        }
    }
}
using System;
using Karmr.Common.Contracts;
using Karmr.Domain.Denormalizers;
using Karmr.Domain.Events;
using Moq;
using NUnit.Framework;
using Karmr.DomainUnitTests.Helpers;

namespace Karmr.DomainUnitTests.Denormalizers
{
    [TestFixture]
    public class ListingDenormalizerTests
    {
        private readonly Mock<IDenormalizerRepository> repository;
        private readonly ListingDenormalizer subject;

        public ListingDenormalizerTests()
        {
            this.repository = new Mock<IDenormalizerRepository>();
            this.subject = new ListingDenormalizer(this.repository.Object);
        }

        [Test]
        public void ApplyingListingCreatedEventCallsRepository()
        {
            var @event = new ListingCreated(Guid.NewGuid(), Guid.NewGuid(), "Name", "Description", DateTime.Now);

            var expectedParams = new
            {
                @event.EntityKey,
                @event.UserId,
                @event.Name,
                @event.Description,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "INSERT INTO ReadModel.Listing ([Id], [UserId], [Name], [Description], [Created]) VALUES (@EntityKey, @UserId, @Name, @Description, @Timestamp)",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }

        [Test]
        public void ApplyingListingUpdatedEventCallsRepository()
        {
            var @event = new ListingUpdated(Guid.NewGuid(), Guid.NewGuid(), "Name", "Description", DateTime.Now);

            var expectedParams = new
            {
                @event.EntityKey,
                @event.Name,
                @event.Description,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "UPDATE ReadModel.Listing SET [Name] = @Name, [Description] = @Description, [Modified] = @Timestamp WHERE Id = @EntityKey",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }
    }
}
using System;
using Karmr.Common.Contracts;
using Karmr.Common.Types;
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
        private Mock<IDenormalizerRepository> repository;
        private ListingDenormalizer subject;

        [SetUp]
        public void Setup()
        {
            this.repository = new Mock<IDenormalizerRepository>();
            this.subject = new ListingDenormalizer(this.repository.Object);
        }

        [Test]
        public void ApplyingListingCreatedEventCallsRepository()
        {
            var @event = new ListingCreated(Guid.NewGuid(), Guid.NewGuid(), "Name", "Description", new GeoLocation(1.23m, 42.123m), DateTime.Now);

            var expectedParams = new
            {
                @event.EntityKey,
                @event.UserId,
                @event.Name,
                @event.Description,
                @event.Location?.Latitude,
                @event.Location?.Longitude,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "INSERT INTO ReadModel.Listing ([Id], [UserId], [Name], [Description], [Latitude], [Longitude], [Created]) VALUES (@EntityKey, @UserId, @Name, @Description, @Latitude, @Longitude, @Timestamp)",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }

        [Test]
        public void ApplyingListingUpdatedEventCallsRepository()
        {
            var @event = new ListingUpdated(Guid.NewGuid(), Guid.NewGuid(), "Name", "Description", new GeoLocation(1.23m, 42.123m), DateTime.Now);

            var expectedParams = new
            {
                @event.EntityKey,
                @event.Name,
                @event.Description,
                @event.Location?.Latitude,
                @event.Location?.Longitude,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "UPDATE ReadModel.Listing SET [Name] = @Name, [Description] = @Description, [Latitude] = @Latitude, [Longitude] = @Longitude, [Modified] = @Timestamp WHERE Id = @EntityKey",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }

        [Test]
        public void ApplyingListingArchivedEventCallsRepository()
        {
            var @event = new ListingArchived(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            var expectedParams = new
            {
                @event.EntityKey,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "UPDATE ReadModel.Listing SET [IsArchived] = 1, [Modified] = @Timestamp WHERE Id = @EntityKey",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }
    }
}
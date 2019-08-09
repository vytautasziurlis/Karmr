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
    public class ListingOfferDenormalizerTests
    {
        private Mock<IDenormalizerRepository> repository;
        private ListingOfferDenormalizer subject;

        [SetUp]
        public void Setup()
        {
            this.repository = new Mock<IDenormalizerRepository>();
            this.subject = new ListingOfferDenormalizer(this.repository.Object);
        }

        [Test]
        public void ApplyingListingOfferCreatedEventCallsRepository()
        {
            var @event = new ListingOfferCreated(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            var expectedParams = new
            {
                @event.OfferId,
                @event.EntityKey,
                @event.UserId,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "INSERT INTO ReadModel.ListingOffer ([Id], [ListingId], [UserId], [Accepted], [Created]) VALUES (@OfferId, @ListingId, @UserId, 0, @Timestamp)",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }

        [Test]
        public void ApplyingListingOfferAcceptedEventCallsRepository()
        {
            var @event = new ListingOfferAccepted(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            var expectedParams = new
            {
                @event.OfferId,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "UPDATE ReadModel.ListingOffer SET [Accepted] = 1, [Modified] = @Timestamp WHERE [Id] = @OfferId",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }
    }
}
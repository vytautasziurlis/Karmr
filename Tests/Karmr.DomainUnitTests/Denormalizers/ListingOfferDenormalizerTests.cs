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
            var @event = new ListingOfferCreated(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            var expectedParams = new
            {
                @event.EntityKey,
                @event.UserId,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "INSERT INTO ReadModel.ListingOffer ([ListingId], [UserId], [Accepted], [Created]) VALUES (@ListingId, @UserId, 0, @Created)",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }
    }
}
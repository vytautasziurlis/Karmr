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
    public class ListingDiscussionDenormalizerTests
    {
        private Mock<IDenormalizerRepository> repository;
        private ListingDiscussionDenormalizer subject;

        [SetUp]
        public void Setup()
        {
            this.repository = new Mock<IDenormalizerRepository>();
            this.subject = new ListingDiscussionDenormalizer(this.repository.Object);
        }

        [Test]
        public void ApplyingListingCreatedEventCallsRepository()
        {
            var @event = new ListingDiscussionPostCreated(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Content", DateTime.Now);

            var expectedParams = new
            {
                @event.PostId,
                @event.EntityKey,
                @event.UserId,
                @event.ThreadId,
                @event.Content,
                @event.Timestamp
            };

            this.subject.Apply(@event);

            this.repository.Verify(
                x => x.Execute(
                    "INSERT INTO ReadModel.ListingDiscussion ([Id], [ListingId], [UserId], [ThreadId], [Content], [Created]) VALUES (@Id, @UserId, @ListingId, @ThreadId, @Content, @Created)",
                    It.Is<object>(@params => Asserts.HaveSameProperties(expectedParams, @params))),
                Times.Once);
        }
    }
}
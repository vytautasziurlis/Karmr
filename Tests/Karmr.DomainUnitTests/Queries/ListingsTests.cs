using System;
using Karmr.Common.Contracts;
using Karmr.Domain.Queries;
using Karmr.Domain.Queries.Models;
using Karmr.DomainUnitTests.Helpers;
using Moq;
using NUnit.Framework;

namespace Karmr.DomainUnitTests.Queries
{
    public class ListingsTests
    {
        private Mock<IQueryRepository> mockRepository;
        private ListingQueries subject;

        [SetUp]
        public void Setup()
        {
            this.mockRepository = new Mock<IQueryRepository>();
            this.subject = new ListingQueries(this.mockRepository.Object);
        }

        [Test]
        public void GetAllCallsRepository()
        {
            this.subject.GetAll();

            this.mockRepository.Verify(x => x.Query<Listing>("SELECT [Id], [Name], [Description], [Created], [Modified] FROM ReadModel.Listing"), Times.Once);
        }

        [Test]
        public void GetByIdCallsRepository()
        {
            var id = Guid.NewGuid();
            this.subject.GetById(id);

            this.mockRepository.Verify(
                x => x.QuerySingle<Listing>("SELECT [Id], [Name], [Description], [Created], [Modified] FROM ReadModel.Listing WHERE [Id] = @Id",
                    It.Is<object>(@params => Asserts.HaveSameProperties(new { id }, @params))),
                Times.Once);
        }
    }
}
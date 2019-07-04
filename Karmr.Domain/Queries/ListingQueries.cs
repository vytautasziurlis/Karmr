using System;
using System.Collections.Generic;
using Karmr.Common.Contracts;
using Karmr.Domain.Queries.Models;

namespace Karmr.Domain.Queries
{
    public sealed class ListingQueries
    {
        private IQueryRepository repository;

        public ListingQueries(IQueryRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Listing> GetAll()
        {
            return this.repository.Query<Listing>("SELECT [Id], [Name], [Description], [Created], [Modified] FROM ReadModel.Listing");
        }

        public Listing GetById(Guid id)
        {
            return this.repository.QuerySingle<Listing>(
                "SELECT [Id], [Name], [Description], [Created], [Modified] FROM ReadModel.Listing WHERE [Id] = @Id",
                new { id });
        }
    }
}
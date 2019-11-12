using System;
using System.Collections.Generic;
using System.Linq;
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
            return this.repository.Query<Listing>("SELECT [Id], [Name], [Description], [LocationName], [Created], [Modified] FROM ReadModel.Listing");
        }

        public IEnumerable<Listing> GetByUserId(Guid userId)
        {
            return this.repository.Query<Listing>(
                "SELECT [Id], [Name], [Description], [LocationName], [Created], [Modified] FROM ReadModel.Listing WHERE [UserId] = @UserId",
                new { userId });
        }

        public ListingDetails GetById(Guid id)
        {
            var result = this.repository.QuerySingle<ListingDetails>(
                "SELECT [Id], [UserId], [Name], [Description], [LocationName], [Latitude], [Longitude], [Created], [Modified] FROM ReadModel.Listing WHERE [Id] = @Id;",
                new {id});

            var discussionItems = this.repository.Query<ListingDiscussionItem>(
                "SELECT [Id], [ThreadId], [UserId], [Content], [Created] FROM ReadModel.ListingDiscussion WHERE ListingId = @Id",
                new { id });

            result.DiscussionThreads = discussionItems
                .GroupBy(
                    x => x.ThreadId,
                    x => new {x.Id, x.ThreadId, x.UserId, x.Content, x.Created},
                    (threadId, posts) => new DiscussionThread(threadId, posts.OrderByDescending(x => x.Created).First().UserId, posts.Select(post => new DiscussionPost(post.UserId, post.Content, post.Created))))
                .ToList();

            return result;
        }

        private class ListingDiscussionItem
        {
            public Guid Id { get; }

            public Guid ThreadId { get; }

            public Guid UserId { get; }

            public string Content { get; }

            public DateTime Created { get; }
        }
    }
}
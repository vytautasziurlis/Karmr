using Karmr.Common.Contracts;
using Karmr.Domain.Events;

namespace Karmr.Domain.Denormalizers
{
    internal sealed class ListingDiscussionDenormalizer : Denormalizer
    {
        internal ListingDiscussionDenormalizer(IDenormalizerRepository repository) : base(repository)
        {
        }

        private void Apply(ListingDiscussionPostCreated @event)
        {
            const string sql = @"INSERT INTO ReadModel.ListingDiscussion ([Id], [ListingId], [UserId], [ThreadId], [Content], [Created]) VALUES (@Id, @ListingId, @UserId, @ThreadId, @Content, @Created)";
            var @params = new
            {
                @event.PostId,
                @event.EntityKey,
                @event.UserId,
                @event.ThreadId,
                @event.Content,
                @event.Timestamp
            };
            this.Repository.Execute(sql, @params);
        }
    }
}
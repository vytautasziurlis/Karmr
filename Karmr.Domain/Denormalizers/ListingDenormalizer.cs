using Karmr.Common.Contracts;
using Karmr.Domain.Events;

namespace Karmr.Domain.Denormalizers
{
    internal sealed class ListingDenormalizer : Denormalizer
    {
        internal ListingDenormalizer(IDenormalizerRepository repository) : base(repository)
        {
        }

        private void Apply(ListingCreated @event)
        {
            const string sql = @"INSERT INTO ReadModel.Listing ([Id], [UserId], [Name], [Description], [Created]) VALUES (@EntityKey, @UserId, @Name, @Description, @Timestamp)";
            var @params = new
                {
                    @event.EntityKey,
                    @event.UserId,
                    @event.Name,
                    @event.Description,
                    @event.Timestamp
                };
            this.Repository.Execute(sql, @params);
        }

        private void Apply(ListingUpdated @event)
        {
            const string sql = @"UPDATE ReadModel.Listing SET [Name] = @Name, [Description] = @Description, [Modified] = @Timestamp WHERE Id = @EntityKey";
            var @params = new
            {
                @event.EntityKey,
                @event.Name,
                @event.Description,
                @event.Timestamp
            };
            this.Repository.Execute(sql, @params);
        }
    }
}
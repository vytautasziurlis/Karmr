using Karmr.Common.Contracts;
using Karmr.Domain.Events;

namespace Karmr.Domain.Denormalizers
{
    internal sealed class ListingOfferDenormalizer : Denormalizer
    {
        internal ListingOfferDenormalizer(IDenormalizerRepository repository) : base(repository)
        {
        }

        private void Apply(ListingOfferCreated @event)
        {
            const string sql = @"INSERT INTO ReadModel.ListingOffer ([Id], [ListingId], [UserId], [Accepted], [Created]) VALUES (@OfferId, @ListingId, @UserId, 0, @Timestamp)";
            var @params = new
            {
                @event.OfferId,
                @event.EntityKey,
                @event.UserId,
                @event.Timestamp
            };
            this.Repository.Execute(sql, @params);
        }

        private void Apply(ListingOfferAccepted @event)
        {
            const string sql = @"UPDATE ReadModel.ListingOffer SET [Accepted] = 1, [Modified] = @Timestamp WHERE [Id] = @OfferId";
            var @params = new
            {
                @event.OfferId,
                @event.Timestamp
            };
            this.Repository.Execute(sql, @params);
        }
    }
}